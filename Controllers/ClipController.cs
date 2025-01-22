using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TwitchClips.Controllers.Parameters;
using TwitchClips.Controllers.Parameters.Enums;
using TwitchClips.InternalLogic;
using TwitchLib.Api;

namespace TwitchClips.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClipController(TwitchAPI twitchAPI, IMapper mapper) : ControllerBase
    {
        private readonly ClipsGetter _clipsGetter = new(twitchAPI, mapper);

        [HttpGet]
        public async Task<ActionResult> GetClipsByGame(string gameId, DateTime? startDate = null, DateTime? endDate = null, DateType? dateType = null) =>
            Ok(await _clipsGetter.GetByGame(gameId, GenerateDateLimits(startDate, endDate, dateType)));

        [HttpGet]
        public async Task<ActionResult> GetAllClipsByGame(string gameId, DateTime? startDate = null, DateTime? endDate = null, DateType? dateType = null) =>
            Ok(await _clipsGetter.GetAllByGame(gameId, GenerateDateLimits(startDate, endDate, dateType)));

        private static DateLimits? GenerateDateLimits(DateTime? startDate = null, DateTime? endDate = null, DateType? dateType = null)
        {
            DateLimits? dateLimits = null;
            if (dateType is not null)
            {
                dateLimits = new(dateType.Value);
            }
            else if (startDate is not null || endDate is not null)
            {
                dateLimits = new(startDate, endDate);
            }

            return dateLimits;
        }
    }
}