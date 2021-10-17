using GladosBank.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GladosBank;

namespace GladosBank.Domain
{
    public class ApplicationContext : DbContext
    {
        DbSet<Account> Accounts { get; set; }
        DbSet<Admin> Admins { get; set; }
        DbSet<Currency> Currency { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<Documentation> Documentations { get; set; }
        DbSet<Information> Informations { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Worker> Workers { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
                : base(options)
        {
        }
    }
}
