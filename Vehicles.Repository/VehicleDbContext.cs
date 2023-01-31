using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using Vehicles.Repository.Entities;
using Vehicles.Repository.Extensions;

namespace Vehicles.Repository
{
    public class VehicleDbContext : DbContext
    {
        public VehicleDbContext(DbContextOptions<VehicleDbContext> options)
            : base(options)
        {
        }
        public DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.RemovePluralizingTableNameConvention();
            AddSeedData(modelBuilder);
        }

        private void AddSeedData(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Make>()
                .HasData(SeedData.Makes);
            modelBuilder.Entity<Model>()
                .HasData(SeedData.Models);
            modelBuilder.Entity<Colour>()
                .HasData(SeedData.Colours);
        }
    }
}
