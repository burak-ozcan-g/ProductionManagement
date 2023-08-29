using Microsoft.EntityFrameworkCore;
using ProductionManagement.Models;

namespace ProductionManagement.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Package> Packages { get; set; }
        public DbSet<ProductPackage> ProductPackages { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductPackage>()
                .HasKey(rml => new { rml.ProductId, rml.PackageId });
            modelBuilder.Entity<ProductPackage>()
                .HasOne(rm => rm.Product)
                .WithMany(rml => rml.ProductPackages)
                .HasForeignKey(rm => rm.ProductId);
            modelBuilder.Entity<ProductPackage>()
                .HasOne(l => l.Package)
                .WithMany(rml => rml.ProductPackages)
                .HasForeignKey(l => l.PackageId);
        }
    }
}
