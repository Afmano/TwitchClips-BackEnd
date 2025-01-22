using AutoMapper;
using EFCore.BulkExtensions;
using TwitchClips.InternalLogic.Contexts;
using TwitchClips.InternalLogic.Testing;
using TwitchClips.Models;
using TwitchLib.Api;
using TwitchLib.Api.Helix.Models.Games;

namespace TwitchClips.InternalLogic.Services
{
    public class ClipsUpdateService(ILogger<ClipsUpdateService> logger, TwitchAPI twitchAPI, IMapper mapper, IServiceScopeFactory scopeFactory) : IHostedService, IDisposable
    {
        private readonly ClipsGetter _clipsGetter = new(twitchAPI, mapper);
        private const int TopGameCount = 5;
        private const long HoursPeriod = 6;
        private readonly TimeSpan _timePeriod = TimeSpan.FromHours(HoursPeriod);
        private int _executionCount = 0;
        private Timer? _timer;

        public Task StartAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Clips Update service started.");
            _timer = new Timer(Update, null, TimeSpan.Zero, _timePeriod);
            return Task.CompletedTask;
        }

        private async void Update(object? state)
        {
            int count = Interlocked.Increment(ref _executionCount);
            logger.LogInformation("Clips update invocked. Total updates during this session: {Count}", count);
            List<Game> topGames;
            using (var watch = new ExecuteStopwatch(logger, "TopGames"))
            {
                topGames = await _clipsGetter.GetTopGames(TopGameCount);
            }

            List<SavedClip> clips = [];
            using (var watch = new ExecuteStopwatch(logger, "1k games clips"))
            {
                var tasks = topGames.Select(async game => await _clipsGetter.GetAllByGame(game.Id));
                var res = await Task.WhenAll(tasks);
                clips = [.. res.SelectMany(list => list).OrderByDescending(clip => clip.ViewCount)];
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