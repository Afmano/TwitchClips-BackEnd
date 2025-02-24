﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TwitchClips.Controllers.Parameters;
using TwitchClips.Controllers.Parameters.Enums;
using TwitchClips.InternalLogic.Twitch;
using TwitchClips.InternalLogic.Twitch.Parameters;
using TwitchClips.Models;
using TwitchLib.Api;

namespace TwitchClips.Controllers
{
    [ApiController, Route("api/[controller]/[action]")]
    public class ClipController(TwitchAPI twitchAPI, IMapper mapper) : ControllerBase
    {
        private readonly ClipsGetter _clipsGetter = new(twitchAPI, mapper);

        [HttpGet]
        public async Task<ActionResult<List<SavedClip>>> GetClips(string id, ClipSource clipSource, DateTime? startDate = null, DateTime? endDate = null, DateType? dateType = null) =>
            Ok(await _clipsGetter.Get(id, clipSource, GenerateDateLimits(startDate, endDate, dateType)));

        [HttpGet]
        public async Task<ActionResult<List<SavedClip>>> GetMaxClips(string id, ClipSource clipSource, DateTime? startDate = null, DateTime? endDate = null, DateType? dateType = null) =>
            Ok(await _clipsGetter.GetMax(id, clipSource, GenerateDateLimits(startDate, endDate, dateType)));

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