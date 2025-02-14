using AutoMapper;
using TwitchClips.Controllers.Parameters;
using TwitchClips.InternalLogic.Twitch.Parameters;
using TwitchClips.Models;
using TwitchLib.Api;
using TwitchLib.Api.Helix.Models.Clips.GetClips;

namespace TwitchClips.InternalLogic.Twitch
{
    public class ClipsGetter(TwitchAPI twitchAPI, IMapper mapper)
    {
        private static int TotalPerRequest => RequestValues.TotalPerRequest;
        private static int TotalLimit => RequestValues.TotalLimit;

        public async Task<List<SavedClip>> Get(string id, ClipSource clipSource, DateLimits? dateLimits = null)
        {
            var getClipsResponse = await GetClips(id, clipSource, dateLimits);
            return mapper.Map<List<SavedClip>>(getClipsResponse.Clips);
        }

        public async Task<List<SavedClip>> GetMax(string id, ClipSource clipSource, DateLimits? dateLimits = null)
        {
            List<Clip> allClips = [];
            string? currentPaginationCursor = null;
            for (int i = 0; i < TotalLimit; i += TotalPerRequest)
            {
                var getClipsResponse = await GetClips(id, clipSource, dateLimits, currentPaginationCursor);
                currentPaginationCursor = getClipsResponse.Pagination.Cursor;
                allClips.AddRange(getClipsResponse.Clips);
            }

            return mapper.Map<List<SavedClip>>(allClips);
        }

        private async Task<GetClipsResponse> GetClips(string id, ClipSource clipSource, DateLimits? dateLimits = null, string? pagination = null)
        {
            return clipSource switch
            {
                ClipSource.Channel => await twitchAPI.Helix.Clips.GetClipsAsync(broadcasterId: id, first: TotalPerRequest, startedAt: dateLimits?.StartDate, endedAt: dateLimits?.EndDate, after: pagination),
                ClipSource.Game => await twitchAPI.Helix.Clips.GetClipsAsync(gameId: id, first: TotalPerRequest, startedAt: dateLimits?.StartDate, endedAt: dateLimits?.EndDate, after: pagination),
                _ => throw new ArgumentException("Invalid clip source", nameof(clipSource)),
            };
        }
    }
}