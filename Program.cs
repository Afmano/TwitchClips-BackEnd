using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NReco.Logging.File;
using TwitchClips.Contexts;
using TwitchClips.Controllers.Parameters;
using TwitchClips.Controllers.Responses.General;
using TwitchClips.InternalLogic;
using TwitchClips.InternalLogic.AppSettings;
using TwitchClips.InternalLogic.Localization;
using TwitchClips.Models.MapProfile;
using TwitchLib.Api;

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
builder.Services.AddMvc().AddJsonOptions(conf =>
{
    conf.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    conf.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
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
TwitchAPI twitchAPI = new();
twitchAPI.Settings.ClientId = appInfo.TwitchSettings.ClientId;
twitchAPI.Settings.Secret = appInfo.TwitchSettings.ClientSecret;
builder.Services.AddSingleton(twitchAPI);
builder.Services.AddAutoMapper(typeof(ClipProfile));

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
    switch (exception)
    {
        case ControllerException:
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new MessageResponse(exception.Message));
            break;

        default:
            await context.Response.WriteAsJsonAsync(new MessageResponse(ResAnswers.ErrorRequest));
            break;
    }
}));
app.UseAuthorization();
app.MapControllers();
app.Logger.LogInformation("App is starting...");
app.Run();