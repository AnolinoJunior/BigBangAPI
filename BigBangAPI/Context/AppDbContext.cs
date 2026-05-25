using BigBangAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BigBangAPI.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Location> Locations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Character>()
                .HasOne(c => c.Location)
                .WithMany(l => l.Characters)
                .HasForeignKey(c => c.LocationId);

            modelBuilder.Entity<CharacterEpisode>()
                .HasKey(ce => new { ce.CharacterId, ce.EpisodeId });

            base.OnModelCreating(modelBuilder);
        }
    }
}