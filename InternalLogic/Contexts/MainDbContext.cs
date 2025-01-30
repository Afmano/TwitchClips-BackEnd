using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TwitchClips.InternalLogic.Converters;
using TwitchClips.Models;

namespace TwitchClips.InternalLogic.Contexts
{
    public class MainDbContext(DbContextOptions<MainDbContext> options) : DbContext(options)
    {
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<SavedClip> SavedClips { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            ArgumentNullException.ThrowIfNull(configurationBuilder);
            configurationBuilder.Properties<DateTime>().HaveConversion<DateTimeAsUtcValueConverter>();
            configurationBuilder.Properties<DateTime?>().HaveConversion<NullableDateTimeAsUtcValueConverter>();
        }
    }
}