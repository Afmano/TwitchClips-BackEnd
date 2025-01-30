using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitchClips.Models
{
    public class SavedClip
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public required string Id { get; init; }

        public required string Url { get; init; }
        public required string EmbedUrl { get; init; }
        public required string BroadcasterId { get; init; }
        public required string BroadcasterName { get; set; }
        public required string CreatorId { get; init; }
        public required string CreatorName { get; set; }
        public required string GameId { get; init; }
        public required string Language { get; set; }
        public required string Title { get; set; }
        public int ViewCount { get; set; }
        public required DateTime CreatedAt { get; init; }
        public required string ThumbnailUrl { get; set; }
        public float Duration { get; set; }
    }
}