using Microsoft.AspNetCore.Mvc;
using TwitchClips.Controllers.Parameters.Enums;
using TwitchClips.Controllers.Responses.Twitch;
using TwitchLib.Api;
using TwitchLib.Api.Helix.Models.Games;
using TwitchLib.Api.Helix.Models.Search;

namespace TwitchClips.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class SearchController(TwitchAPI twitchAPI) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> Search(string searchValue, SearchFlags searchType = SearchFlags.All)
        {
            List<Channel>? channels = [];
            List<Game>? games = [];
            if (searchType.HasFlag(SearchFlags.Channel))
            {
                var channelsRes = await twitchAPI.Helix.Search.SearchChannelsAsync(searchValue);
                channels = [.. channelsRes.Channels];
            }

            if (searchType.HasFlag(SearchFlags.Category))
            {
                var categoriesRes = await twitchAPI.Helix.Search.SearchCategoriesAsync(searchValue);
                games = [.. categoriesRes.Games];
            }

            return Ok(new SearchResponse(channels, games));
        }
    }
}