using System.ComponentModel.DataAnnotations;

namespace TwitchClips.Models
{
    public class Playlist
    {
        [Key]
        public int Id { get; private init; }

        public required string Name { get; set; }
        public required List<string> ClipIds { get; set; }
        public DateTime CreationDate { get; init; }
        public DateTime LastChangeDate { get; set; }
    }
}