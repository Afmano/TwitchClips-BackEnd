using TwitchLib.Api.Helix.Models.Channels.GetChannelInformation;
using TwitchLib.Api.Helix.Models.Users.GetUsers;

namespace TwitchClips.Controllers.Responses.Twitch
{
    public record ChannelResponse(ChannelInformation? ChannelInformation, User? User);
}