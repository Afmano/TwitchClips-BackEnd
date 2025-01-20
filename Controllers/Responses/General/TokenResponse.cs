namespace TwitchClips.Controllers.Responses.General
{
    public record TokenResponse(string Token, DateTime ExparationDateTime, string CookieName = "session");
}