namespace TwitchClips.InternalLogic.Responses
{
    public record PaginateCollectionResponse<T>(List<T> Collection, int Count);
}