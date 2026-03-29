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
        public DbSet<RideSession> RideSessions { get; set; }
        public DbSet<RideRequests> RideRequests { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<DriverHistory> DriverHistories { get; set; }
        public DbSet<DriverHistoryPassenger> DriverHistoryPassengers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RideSession>(e => {
                e.HasKey(r => r.Id);

                e.Property(r => r.Id)
                 .ValueGeneratedOnAdd();

                e.Property(r => r.PickupLatitude).IsRequired();
                e.Property(r => r.PickupLongitude).IsRequired();
                e.Property(r => r.PickupName).IsRequired().HasMaxLength(500);

                e.Property(r => r.DestinationLatitude).IsRequired();
                e.Property(r => r.DestinationLongitude).IsRequired();
                e.Property(r => r.DestinationName).IsRequired().HasMaxLength(500);

                e.HasOne(r => r.Driver)
                 .WithMany()
                 .HasForeignKey(r => r.DriverId)
                 .OnDelete(DeleteBehavior.NoAction);

                e.HasOne(r => r.Vehicle)
                 .WithMany()
                 .HasForeignKey(r => r.VehicleId)
                 .OnDelete(DeleteBehavior.NoAction);

                e.HasIndex(r => new { r.DriverId, r.IsActive })
                 .HasDatabaseName("IX_RideSessions_DriverActive");
            });

            modelBuilder.Entity<RideRequests>(e =>
            {
                e.HasKey(r => r.Id);

                e.Property(r => r.Id).ValueGeneratedOnAdd();
                e.Property(r => r.PickupLatitude).IsRequired();
                e.Property(r => r.PickupLongitude).IsRequired();
                e.Property(r => r.PickupName).IsRequired();

                e.Property(r => r.DestinationLatitude).IsRequired();
                e.Property(r => r.DestinationLongitude).IsRequired();
                e.Property(r => r.DestinationName).IsRequired();

                e.Property(r => r.RespondedAt).IsRequired(false);

                e.HasOne(r => r.Passenger)
                 .WithMany()
                 .HasForeignKey(r => r.PassengerId)
                 .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Booking>(e =>
            {
                e.HasKey(b => b.BookingId);

                e.Property(b => b.BookingId)
                 .ValueGeneratedOnAdd();

                e.Property(b => b.PIN)
                 .HasMaxLength(6);
                e.Property(b => b.Fare).HasPrecision(10, 2)
                .HasDefaultValue(0m);

                e.HasOne(b => b.RideRequest)
                 .WithMany()
                 .HasForeignKey(b => b.RideRequestId)
                 .OnDelete(DeleteBehavior.NoAction);

                e.HasOne(b => b.RideSession)
                 .WithMany()
                 .HasForeignKey(b => b.SessionId)
                 .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<DriverHistory>(e => {
                e.HasKey(h => h.DriverHistoryId);
                e.Property(h => h.DriverHistoryId).ValueGeneratedOnAdd();

                e.Property(h => h.StartingLocation).IsRequired().HasMaxLength(500);
                e.Property(h => h.DestinationLocation).IsRequired().HasMaxLength(500);
                e.Property(h => h.TotalFare).HasPrecision(10, 2);

                e.HasOne(h => h.RideSession)
                 .WithMany()
                 .HasForeignKey(h => h.RideSessionId)
                 .OnDelete(DeleteBehavior.NoAction);

                e.HasOne(h => h.Driver)
                 .WithMany()
                 .HasForeignKey(h => h.DriverId)
                 .OnDelete(DeleteBehavior.NoAction);

                e.HasMany(h => h.Passengers)
                 .WithOne(p => p.DriverHistory)
                 .HasForeignKey(p => p.DriverHistoryId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasIndex(h => h.DriverId)
                 .HasDatabaseName("IX_DriverHistories_DriverId");

                e.HasIndex(h => h.RideSessionId)
                 .IsUnique()
                 .HasDatabaseName("IX_DriverHistories_RideSessionId");
            });

            modelBuilder.Entity<DriverHistoryPassenger>(e => {
                e.HasKey(p => p.PassengerHistoryId);
                e.Property(p => p.PassengerHistoryId).ValueGeneratedOnAdd();

                e.Property(p => p.PassengerName).IsRequired().HasMaxLength(256);
                e.Property(p => p.Pickup).IsRequired().HasMaxLength(500);
                e.Property(p => p.Fare).HasPrecision(10, 2);
                e.Property(p => p.Ratings).HasPrecision(3, 1);

                e.HasOne(p => p.DriverHistory)
                 .WithMany(h => h.Passengers)
                 .HasForeignKey(p => p.DriverHistoryId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(p => p.RideRequests)
                 .WithMany()
                 .HasForeignKey(p => p.RideId)
                 .OnDelete(DeleteBehavior.NoAction);

                e.HasIndex(p => p.DriverHistoryId)
                 .HasDatabaseName("IX_DriverHistoryPassengers_DriverHistoryId");
            });
        }
    }
}
