using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TwitchClips.InternalLogic.Converters
{
    //Source: https://stackoverflow.com/a/73154546/18599560
    public class DateTimeAsUtcValueConverter() : ValueConverter<DateTime, DateTime>(v =>
        v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(), v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
}