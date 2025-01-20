namespace TwitchClips.Controllers.Parameters.Enums
{
    [Flags]
    public enum SearchFlags
    {
        Channel = 1,
        Category = 2,
        All = Channel | Category
    }
}