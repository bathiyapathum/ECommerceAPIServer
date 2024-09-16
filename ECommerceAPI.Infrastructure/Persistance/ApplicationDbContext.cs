using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Persistance
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Define DbSet properties for each entity (table) in the database
        //public DbSet<Product> Products { get; set; }
        //public DbSet<Order> Orders { get; set; }

        // Override OnModelCreating to configure the model relationships, constraints, etc.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Custom configurations (if needed)
            // modelBuilder.Entity<Product>().HasKey(p => p.ProductId);
        }
    }
}
