using AutoMapper;
using EFCore.BulkExtensions;
using TwitchClips.Controllers.Parameters;
using TwitchClips.Controllers.Parameters.Enums;
using TwitchClips.InternalLogic.Contexts;
using TwitchClips.InternalLogic.Testing;
using TwitchClips.InternalLogic.Twitch;
using TwitchClips.InternalLogic.Twitch.Parameters;
using TwitchClips.Models;
using TwitchLib.Api;
using TwitchLib.Api.Helix.Models.Games;

namespace TwitchClips.InternalLogic.Services
{
    public class ClipsUpdateService(ILogger<ClipsUpdateService> logger, TwitchAPI twitchAPI, IMapper mapper,
        IServiceScopeFactory scopeFactory) : IHostedService, IDisposable
    {
        private readonly ClipsGetter _clipsGetter = new(twitchAPI, mapper);
        private readonly GameGetter _gameGetter = new(twitchAPI);

        #region Testing values

        private const int TopGameCount = 5;
        private const long StartAfterMins = 30;
        private const long HoursPeriod = 6;
        private readonly DateLimits _dateLimits = new(DateType.AllTime);

        #endregion Testing values

        private readonly TimeSpan _startAfterTime = TimeSpan.FromMinutes(StartAfterMins);
        private readonly TimeSpan _executionPeriodTime = TimeSpan.FromHours(HoursPeriod);
        private int _executionCount = 0;
        private Timer? _timer;

        public Task StartAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Clips Update service started.");
            _timer = new Timer(Update, null, _startAfterTime, _executionPeriodTime);
            return Task.CompletedTask;
        }

        private async void Update(object? state)
        {
            int count = Interlocked.Increment(ref _executionCount);
            logger.LogInformation("Clips update invocked. Total updates during this session: {Count}", count);
            List<Game> topGames;
            using (var watch = new ExecuteStopwatch(logger, "TopGames"))
            {
                topGames = await _gameGetter.GetTopGames(TopGameCount);
            }

            List<SavedClip> clips = [];
            using (var watch = new ExecuteStopwatch(logger, "1k games clips"))
            {
                var tasks = topGames.Select(async game => await _clipsGetter.GetMax(game.Id, ClipSource.Game, _dateLimits));
                var res = await Task.WhenAll(tasks);
                clips = [.. res.SelectMany(list => list).OrderByDescending(clip => clip.ViewCount)
                    .DistinctBy(clip => clip.Id)];
            }

            logger.LogInformation("Clips received: {Count}", clips.Count);
            using var scope = scopeFactory.CreateScope();
            MainDbContext dbContext = scope.ServiceProvider.GetRequiredService<MainDbContext>();
            await dbContext.BulkInsertOrUpdateAsync(clips);
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Clips Update service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose() => _timer?.Dispose();
    }
}