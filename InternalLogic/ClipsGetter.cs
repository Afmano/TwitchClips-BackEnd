using AutoMapper;
using TwitchClips.Controllers.Parameters;
using TwitchClips.Models;
using TwitchLib.Api;

namespace TwitchClips.InternalLogic
{
    public class ClipsGetter(TwitchAPI twitchAPI, IMapper mapper)
    {
        private const int MAX_CLIPS_PER_REQ = 100;
        private const int MAX_CLIPS = 1000;

        public async Task<List<SavedClip>> GetByGame(string gameId, DateLimits? dateLimits)
        {
            var getClipsResponse = await twitchAPI.Helix.Clips.GetClipsAsync(gameId: gameId, first: 100, startedAt: dateLimits?.StartDate, endedAt: dateLimits?.EndDate);
            return mapper.Map<List<SavedClip>>(getClipsResponse.Clips);
        }

        public async Task<List<SavedClip>> GetAllByGame(string gameId, DateLimits? dateLimits)
        {
            List<SavedClip> allClips = [];
            string? currentPaginationCursor = null;
            for (int i = 0; i < MAX_CLIPS; i += MAX_CLIPS_PER_REQ)
            {
                var getClipsResponse = await twitchAPI.Helix.Clips.GetClipsAsync(gameId: gameId, first: MAX_CLIPS_PER_REQ, after: currentPaginationCursor, startedAt: dateLimits?.StartDate, endedAt: dateLimits?.EndDate);
                currentPaginationCursor = getClipsResponse.Pagination.Cursor;
                allClips.AddRange(mapper.Map<List<SavedClip>>(getClipsResponse.Clips));
            }

            return allClips;
        }
    }
}