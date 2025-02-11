using Microsoft.AspNetCore.Mvc;
using TwitchClips.Controllers.Responses.Twitch;
using TwitchLib.Api;
using TwitchLib.Api.Helix.Models.Channels.GetChannelInformation;
using TwitchLib.Api.Helix.Models.Users.GetUsers;

namespace TwitchClips.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class ChannelController(TwitchAPI twitchAPI) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<ChannelResponse>> Get(string id)
        {
            ChannelInformation? resultChannel = (await twitchAPI.Helix.Channels.GetChannelInformationAsync(id)).Data.FirstOrDefault();
            User? resultUser = (await twitchAPI.Helix.Users.GetUsersAsync([id])).Users.FirstOrDefault();
            if (resultUser == null && resultChannel == null)
            {
                return NotFound();
            }

            return Ok(new ChannelResponse(resultChannel, resultUser));
        }
    }
}