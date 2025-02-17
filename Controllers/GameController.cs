using Microsoft.AspNetCore.Mvc;
using TwitchClips.InternalLogic.Twitch;
using TwitchLib.Api;
using TwitchLib.Api.Helix.Models.Games;

namespace TwitchClips.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class GameController(TwitchAPI twitchAPI) : ControllerBase
    {
        private readonly GameGetter _gameGetter = new(twitchAPI);

        [HttpGet]
        public async Task<ActionResult<List<Game>>> Get(int count = 20) =>
            Ok(await _gameGetter.GetTopGames(count));

        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> Get(string id)
        {
            Game? resultGame = await _gameGetter.GetGame(id);
            if (resultGame == null)
            {
                return NotFound();
            }

            return Ok(resultGame);
        }
    }
}