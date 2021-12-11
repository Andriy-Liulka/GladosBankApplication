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
        
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Currency> Currency { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Information> Informations { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Worker> Workers { get; set; }
        public virtual DbSet<OperationsHistory> OperationsHistory { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options){}
        public ApplicationContext(){}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var userModel = builder.Entity<User>();
            var workerModel = builder.Entity<Worker>();

            userModel.Property(x => x.IsActive)
                .HasDefaultValue(true)
                .ValueGeneratedNever();

            workerModel.Property(x => x.Salary)
                .HasDefaultValue(0.0)
                .ValueGeneratedNever();

            builder.Entity<Currency>().HasData(
                new Currency {Code= "EUR",Symbol= "€" },
                new Currency { Code = "UAN", Symbol = "₴" },
                new Currency { Code = "USD", Symbol = "$" }
                );

            builder.Entity<User>().HasData(
                new User { Email="admin@example.com",IsActive=true,Phone="06866414",PasswordHash= "5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5",Login="admin" ,Id=1024}
            );
            builder.Entity<Admin>().HasData(
                new Admin {Id=1,UserId=1024}
            );

            base.OnModelCreating(builder);


        }

    }
}
