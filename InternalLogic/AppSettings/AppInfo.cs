namespace TwitchClips.InternalLogic.AppSettings
{
    public record AppInfo
    {
        public required string ApplicationName { get; init; }
        public required string Version { get; init; }
        public required string Host { get; init; }
        public required AuthSettings AuthSettings { get; init; }
    }
}