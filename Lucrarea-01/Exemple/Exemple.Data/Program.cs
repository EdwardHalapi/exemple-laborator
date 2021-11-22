using Example.Data.Models;
using Exemple.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Data
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
           
        }

        public DbSet<Order> OrderHeader { get; set; }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Product>().("Product").HasKey(s => s.ProductId);
            //modelBuilder.Entity<Order>().ToTable("Order").HasKey(s => s.OrderId);
        }
    }
}
