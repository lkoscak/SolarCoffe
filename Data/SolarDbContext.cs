using Microsoft.EntityFrameworkCore;
using SolarCoffe.Data.Models;
using System;

namespace SolarCoffe.Data
{
    public class SolarDbContext : DbContext
    {
         public SolarDbContext()
        {
        }
        public SolarDbContext(DbContextOptions options) : base(options)
        {
        }
        public virtual DbSet<CustomerAddress> CustomerAddress { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
    }
}
