using System.Diagnostics;

namespace TwitchClips.InternalLogic.Testing
{
    public class ExecuteStopwatch : IDisposable
    {
        private readonly Stopwatch _stopwatch = new();
        private readonly ILogger _logger;
        private readonly string _stopwatchName;

        public ExecuteStopwatch(ILogger logger, string stopwatchName)
        {
            _logger = logger;
            _stopwatchName = stopwatchName;
            _stopwatch.Start();
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            _logger.LogInformation("Time elapsed during \"{Name}\": {Elapsed}", _stopwatchName, _stopwatch.Elapsed);
        }
    }
}