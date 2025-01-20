using TwitchLib.Api.Helix.Models.Games;
using TwitchLib.Api.Helix.Models.Search;

namespace TwitchClips.Controllers.Responses.Twitch
{
    public record SearchResponse(List<Channel> Channels, List<Game> Games);
}