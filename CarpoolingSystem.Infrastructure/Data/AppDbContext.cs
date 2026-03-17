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

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            // RideSession Table 
            modelBuilder.Entity<RideSession>(e => {
                e.HasKey(r => r.Id);

                e.Property(r => r.Id)
                 .ValueGeneratedOnAdd(); 

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
                 .HasMaxLength(4);

                e.HasOne(b => b.RideRequest)
                 .WithMany()
                 .HasForeignKey(b => b.RideRequestId)
                 .OnDelete(DeleteBehavior.NoAction);

                e.HasOne(b => b.RideSession)
                 .WithMany()
                 .HasForeignKey(b => b.SessionId)
                 .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
