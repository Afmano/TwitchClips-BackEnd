namespace TwitchClips.InternalLogic.Responses
{
    public record TokenResponse(string Token, DateTime ExparationDateTime, string CookieName = "session");
}