using Microsoft.EntityFrameworkCore;
using TwitchClips.Models;

namespace TwitchClips.Contexts
{
    public class MainDbContext(DbContextOptions<MainDbContext> options) : DbContext(options)
    {
        public DbSet<Playlist> Playlists { get; set; }
    }
}