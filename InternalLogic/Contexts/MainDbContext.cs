using Microsoft.EntityFrameworkCore;
using TwitchClips.Models;

namespace TwitchClips.InternalLogic.Contexts
{
    public class MainDbContext(DbContextOptions<MainDbContext> options) : DbContext(options)
    {
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<SavedClip> SavedClips { get; set; }
    }
}