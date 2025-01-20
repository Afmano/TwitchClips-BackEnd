using TwitchClips.Controllers.Parameters.Enums;

namespace TwitchClips.Controllers.Parameters
{
    public class DateLimits
    {
        public DateTime? StartDate;
        public DateTime? EndDate;

        public DateLimits(DateTime? startDate, DateTime? endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        public DateLimits(DateType type)
        {
            StartDate = StartDateByType(type);
        }

        private static DateTime? StartDateByType(DateType type)
        {
            return type switch
            {
                DateType.Today => DateTime.UtcNow.AddDays(-1),
                DateType.Week => DateTime.UtcNow.AddDays(-7),
                DateType.Month => DateTime.UtcNow.AddMonths(-1),
                DateType.Year => DateTime.UtcNow.AddYears(-1),
                DateType.FiveYears => DateTime.UtcNow.AddYears(-5),
                _ => null
            };
        }
    }
}