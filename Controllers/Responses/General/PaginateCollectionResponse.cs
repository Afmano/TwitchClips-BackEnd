namespace TwitchClips.Controllers.Responses.General
{
    public record PaginateCollectionResponse<T>(List<T> Collection, int Count);
}