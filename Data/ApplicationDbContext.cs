
using Microsoft.EntityFrameworkCore;
using MatchVault.Models;

namespace MatchVault.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Match> Matches { get; set; } = null!;
        public DbSet<Wrestler> Wrestlers { get; set; } = null!;
        public DbSet<Event> Events { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Connect matches to wrestling events
            modelBuilder.Entity<Match>()
                .HasOne(m => m.WrestlingEvent)
                .WithMany()
                .HasForeignKey(m => m.EventId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
