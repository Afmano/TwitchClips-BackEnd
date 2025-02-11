using Microsoft.AspNetCore.Mvc;
using TwitchLib.Api;
using TwitchLib.Api.Helix.Models.Games;

namespace TwitchClips.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class GameController(TwitchAPI twitchAPI) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> Get(string id)
        {
            Game? resultGame = (await twitchAPI.Helix.Games.GetGamesAsync([id])).Games.FirstOrDefault();
            if (resultGame == null)
            {
                return NotFound();
            }

            return Ok(resultGame);
        }
    }
}