namespace TwitchClips.Controllers.Parameters.Enums
{
    //Twitch throw empty response, while attempting to get clips earlier than 2017 year. So options for DataType is limited.
    public enum DateType
    {
        Today,
        Week,
        Month,
        Year,
        FiveYears,
        AllTime
    }
}