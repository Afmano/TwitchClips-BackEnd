using AutoMapper;
using TwitchClips.Controllers.Parameters;
using TwitchClips.Models;
using TwitchLib.Api;
using TwitchLib.Api.Helix.Models.Games;

namespace TwitchClips.InternalLogic
{
    public class ClipsGetter(TwitchAPI twitchAPI, IMapper mapper)
    {
        private const int TotalPerRequest = 100;
        private const int TotalLimit = 1000;

        public async Task<List<SavedClip>> GetByGame(string gameId, DateLimits? dateLimits = null)
        {
            var getClipsResponse = await twitchAPI.Helix.Clips.GetClipsAsync(gameId: gameId, first: 100, startedAt: dateLimits?.StartDate, endedAt: dateLimits?.EndDate);
            return mapper.Map<List<SavedClip>>(getClipsResponse.Clips);
        }

        public async Task<List<SavedClip>> GetAllByGame(string gameId, DateLimits? dateLimits = null)
        {
            List<SavedClip> allClips = [];
            string? currentPaginationCursor = null;
            for (int i = 0; i < TotalLimit; i += TotalPerRequest)
            {
                var getClipsResponse = await twitchAPI.Helix.Clips.GetClipsAsync(gameId: gameId, first: TotalPerRequest, after: currentPaginationCursor, startedAt: dateLimits?.StartDate, endedAt: dateLimits?.EndDate);
                currentPaginationCursor = getClipsResponse.Pagination.Cursor;
                allClips.AddRange(mapper.Map<List<SavedClip>>(getClipsResponse.Clips));
            }

            return allClips;
        }

        public async Task<List<Game>> GetTopGames(int gamesLimit = TotalLimit, int perRequest = TotalPerRequest)
        {
            List<Game> topGames = [];
            string? currentPaginationCursor = null;
            if (perRequest > TotalPerRequest)
            {
                perRequest = TotalPerRequest;
            }

            if (gamesLimit < perRequest)
            {
                perRequest = gamesLimit;
            }

            for (int i = 0; i < gamesLimit; i += TotalPerRequest)
            {
                var topGamesResponse = await twitchAPI.Helix.Games.GetTopGamesAsync(first: perRequest, after: currentPaginationCursor);
                currentPaginationCursor = topGamesResponse.Pagination.Cursor;
                topGames.AddRange(topGamesResponse.Data);
            }

            return topGames;
        }
    }
}