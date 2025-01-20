namespace TwitchClips.Controllers.Responses.General
{
    public record EnumResponse(List<EnumValue> EnumValues);
    public record EnumValue(int Id, string Value);
}