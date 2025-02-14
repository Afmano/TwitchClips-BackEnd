using TwitchLib.Api;
using TwitchLib.Api.Helix.Models.Games;

namespace TwitchClips.InternalLogic.Twitch
{
    public class GameGetter(TwitchAPI twitchAPI)
    {
        public async Task<List<Game>> GetTopGames(int gamesLimit = RequestValues.TotalLimit, int perRequest = RequestValues.TotalPerRequest)
        {
            List<Game> topGames = [];
            string? currentPaginationCursor = null;
            if (perRequest > RequestValues.TotalPerRequest)
            {
                perRequest = RequestValues.TotalPerRequest;
            }

            if (gamesLimit < perRequest)
            {
                perRequest = gamesLimit;
            }

            for (int i = 0; i < gamesLimit; i += perRequest)
            {
                var topGamesResponse = await twitchAPI.Helix.Games.GetTopGamesAsync(first: perRequest, after: currentPaginationCursor);
                currentPaginationCursor = topGamesResponse.Pagination.Cursor;
                topGames.AddRange(topGamesResponse.Data);
            }

            return topGames;
        }
    }
}