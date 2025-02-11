namespace TwitchClips.Controllers.Parameters.Enums
{
    [Flags]
    public enum SearchFlags
    {
        Channel = 1,
        Game = 2,
        All = Channel | Game
    }
}