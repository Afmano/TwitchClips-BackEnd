﻿using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TwitchClips.InternalLogic.Converters
{
    //Source: https://stackoverflow.com/a/73154546/18599560
    public class NullableDateTimeAsUtcValueConverter() : ValueConverter<DateTime?, DateTime?>(v =>
             !v.HasValue ? v : ToUtc(v.Value), v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v)
    {
        private static DateTime? ToUtc(DateTime v) => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime();
    }
}