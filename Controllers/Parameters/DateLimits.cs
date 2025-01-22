using TwitchClips.Controllers.Parameters.Enums;

namespace TwitchClips.Controllers.Parameters
{
    public class DateLimits
    {
        public DateTime? StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }

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