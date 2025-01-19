namespace TwitchClips.InternalLogic.Responses
{
    public record EnumResponse(List<EnumValue> EnumValues);
    public record EnumValue(int Id, string Value);
}