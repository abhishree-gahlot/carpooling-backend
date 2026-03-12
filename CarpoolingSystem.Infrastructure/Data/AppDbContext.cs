using CarpoolingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarpoolingSystem.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<RideSession> RideSessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            // ── Location Table ────────────────────────────────────────
            modelBuilder.Entity<Location>(e => {
                e.HasKey(l => l.ID);

                e.HasOne(l => l.User)
                 .WithOne()
                 .HasForeignKey<Location>(l => l.UserId)
                 .OnDelete(DeleteBehavior.NoAction);
            });

            // ── RideSession Table ─────────────────────────────────────
            modelBuilder.Entity<RideSession>(e => {
                e.HasKey(r => r.Id);

                // Driver FK → Users
                e.HasOne(r => r.Driver)
                 .WithMany()
                 .HasForeignKey(r => r.DriverId)
                 .OnDelete(DeleteBehavior.NoAction);

                // Vehicle FK → Vehicles
                e.HasOne(r => r.Vehicle)
                 .WithMany()
                 .HasForeignKey(r => r.VehicleId)
                 .OnDelete(DeleteBehavior.NoAction);

                // Passenger FK → Users (nullable)
                e.HasOne(r => r.Passenger)
                 .WithMany()
                 .HasForeignKey(r => r.PassengerId)
                 .OnDelete(DeleteBehavior.NoAction)
                 .IsRequired(false);

                e.HasIndex(r => new { r.DriverId, r.IsActive })
                 .HasDatabaseName("IX_RideSessions_DriverActive");
            });
        }
    }
}
