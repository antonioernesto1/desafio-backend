using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MotorcycleRental.Domain.Aggregates.DeliveryDriver;
using MotorcycleRental.Domain.Aggregates.Motorcycle;
using MotorcycleRental.Domain.Aggregates.Rental;

namespace MotorcycleRental.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Motorcycle> Motorcycles { get; set; }
        public DbSet<DeliveryDriver> DeliveryDrivers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DeliveryDriver>(dd =>
            {
                dd.OwnsOne(d => d.Cnh);

                dd.HasIndex(d => d.Cnpj).IsUnique();
                dd.HasIndex(d => d.Cnh.CnhNumber).IsUnique();
            });

            modelBuilder.Entity<Motorcycle>()
                .HasIndex(m => m.LicensePlate)
                .IsUnique();
        }
    }
}
