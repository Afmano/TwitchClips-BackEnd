using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwitchClips.Controllers.Parameters.Enums;
using TwitchClips.Controllers.Responses.General;
using TwitchClips.InternalLogic.Contexts;
using TwitchClips.InternalLogic.Localization;
using TwitchClips.Models;

namespace TwitchClips.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class SavedClipController(MainDbContext dbContext) : ControllerBase
    {
        [HttpGet]
        public ActionResult<IOrderedQueryable<SavedClip>> Get(int pageIndex = 0, int pageSize = 100,
            OrderType orderType = OrderType.Descending, OrderBy orderBy = OrderBy.Views)
        {
            object orderSelector(SavedClip x) => orderBy switch
            {
                OrderBy.Views => x.ViewCount,
                OrderBy.Time => x.CreatedAt,
                _ => throw new ArgumentException("Unknown type of a ordering by value", nameof(orderBy)),
            };
            var orderedCollection = orderType switch
            {
                OrderType.Ascending => dbContext.SavedClips.OrderBy(orderSelector),
                OrderType.Descending => dbContext.SavedClips.OrderByDescending(orderSelector),
                _ => throw new ArgumentException("Unknown type of a ordering", nameof(orderType)),
            };
            return Ok(orderedCollection.Skip(pageIndex * pageSize).Take(pageSize));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SavedClip>> Get(string id)
        {
            var result = await dbContext.SavedClips.FirstOrDefaultAsync(x => x.Id == id);
            if (result == null)
            {
                return NotFound(new MessageResponse(ResAnswers.NotFoundNullEntity));
            }

            return Ok(result);
        }
    }
}