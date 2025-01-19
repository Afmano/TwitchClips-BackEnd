using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NReco.Logging.File;
using TwitchClips.Contexts;
using TwitchClips.InternalLogic;
using TwitchClips.InternalLogic.AppSettings;
using TwitchClips.InternalLogic.Localization;
using TwitchClips.InternalLogic.Responses;

var builder = WebApplication.CreateBuilder(args);
IConfiguration appInfoSection = builder.Configuration.GetSection("AppInfo");
AppInfo appInfo = appInfoSection.Get<AppInfo>() ?? throw new NullReferenceException("Configure appsettings.json");
builder.Services.Configure<AppInfo>(appInfoSection);
builder.Services.AddControllers();
builder.Services.AddCors();
builder.Services.AddSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1", new OpenApiInfo { Title = appInfo.ApplicationName, Version = appInfo.Version });
    setup.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter into field the word 'Bearer' following by space and JWT",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });
    setup.AddSecurityRequirement(new OpenApiSecurityRequirement(){
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
            Scheme = "oauth2",
            Name = "Bearer",
            In = ParameterLocation.Header,
          }, new List<string>()
        }
    });
});
builder.Services.AddMvc().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
builder.Services.AddLogging(conf =>
{
    conf.AddConsole();
    conf.AddFile(builder.Configuration.GetSection("Logging"));
});
builder.Services.AddDbContext<MainDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("MainDb")));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer("Bearer", options =>
    {
        options.Authority = appInfo.Host;
        if (builder.Environment.IsDevelopment())
        {
            options.RequireHttpsMetadata = false;
        }

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = appInfo.Host,
            ValidAudience = appInfo.AuthSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appInfo.AuthSettings.IssuerSigningKey))
        };
    });

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed((host) => true)
                .AllowCredentials()
            );
}

app.UseHttpsRedirection();
app.UseExceptionHandler(builder => builder.Run(async context =>
{
    Exception? exception = context.Features
        ?.Get<IExceptionHandlerPathFeature>()
        ?.Error;
    app.Logger.LogError(exception, "Intercepted error.");
    if (exception is ControllerException)
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsJsonAsync(new MessageResponse(exception.Message));
    }
    else
    {
        await context.Response.WriteAsJsonAsync(new MessageResponse(ResAnswers.ErrorRequest));
    }
}));
app.UseAuthorization();
app.MapControllers();
app.Logger.LogInformation("App is starting...");
app.Run();