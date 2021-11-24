using GladosBank.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GladosBank;
using Microsoft.Extensions.Configuration;

namespace GladosBank.Domain
{
    public class ApplicationContext : DbContext
    {
        
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Currency> Currency { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Information> Informations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<OperationsHistory> OperationsHistory { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options){}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //var userModel = builder.Entity<User>();
            //var workerModel = builder.Entity<Worker>();

            //userModel.Property(x => x.IsActive).HasDefaultValue(true);
            //workerModel.Property(x => x.Salary).HasDefaultValue(0.0);

            //base.OnModelCreating(builder);
        }

    }
}
