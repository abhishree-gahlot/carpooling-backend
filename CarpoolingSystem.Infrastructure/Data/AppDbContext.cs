using CarpoolingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarpoolingSystem.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<RideSession> RideSessions { get; set; }
    public DbSet<RideRequests> RideRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<RideSession>(e =>
        {
            e.HasKey(r => r.Id);
            e.Property(r => r.Id).ValueGeneratedOnAdd();

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

            e.HasOne(r => r.Passenger)
             .WithMany()
             .HasForeignKey(r => r.PassengerId)
             .OnDelete(DeleteBehavior.NoAction);
        });
    }
}