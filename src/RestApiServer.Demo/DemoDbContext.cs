using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RestApiServer.Demo
{
    public class Customer
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }

        public virtual List<Order> Orders { get; set; } = new List<Order>();
    }

    public class Order
    {
        [Key]
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public long CustomerId { get; set; }
    }

    public class DemoDbContext : DbContext
    {
        public DemoDbContext(DbContextOptions<DemoDbContext> options) : base(options)
        { 
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
