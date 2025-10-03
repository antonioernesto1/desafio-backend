using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MotorcycleRental.Domain.Aggregates.DeliveryDrivers;
using MotorcycleRental.Domain.Aggregates.Motorcycles;
using MotorcycleRental.Domain.Aggregates.Rentals;

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
                dd.HasIndex(d => d.Cnpj).IsUnique();

                dd.OwnsOne(d => d.Cnh, cnhBuilder =>
                {
                    cnhBuilder.HasIndex(c => c.Number).IsUnique();
                });
            });

            modelBuilder.Entity<Motorcycle>()
                .HasIndex(m => m.LicensePlate)
                .IsUnique();

            modelBuilder.Entity<Motorcycle>()
                .HasMany(m => m.Rentals)
                .WithOne(r => r.Motorcycle)
                .HasForeignKey(r => r.MotorcycleId);
        }
    }
}
