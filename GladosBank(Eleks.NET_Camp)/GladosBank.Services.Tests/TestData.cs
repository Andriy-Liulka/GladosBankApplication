using GladosBank.Domain;
using GladosBank.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Services.Tests
{
    public static class TestData
    {
        public static IList<Currency> GetTestCurrencies()
        {
            var currenies = new List<Currency>
            {
                new Currency{Code="UAN",Symbol="₴",Coefficient=1 },
                new Currency{Code="USD",Symbol="$",Coefficient=28 },
                new Currency{Code="EUR",Symbol="€",Coefficient=32 }
            };
            return currenies;
        }
        public static IList<User> GetTestUsers()
        {
            var users = new List<User>
            {
                new User{
                        Id=1,
                        Phone="111314181",
                        Email="gdfg@example.com",
                        Login="Vasya",
                        PasswordHash="12345",
                        IsActive=true },
                new User{
                        Id=2,
                        Phone="111314181",
                        Email="gdfg@example.com",
                        Login="Vitaliy",
                        PasswordHash="12345",
                        IsActive=true },
                new User{
                        Id=3,
                        Phone="111314181",
                        Email="gdfg@example.com",
                        Login="Nikita",
                        PasswordHash="12345",
                        IsActive=false }

            };
            return users;
        }
        public static IList<Admin> GetTestAdmins()
        {
            var admins = new List<Admin>()
            {
                new Admin()
                {
                    Id=1,
                    UserId=1
                }
            };
            return admins;
        }
        public static IList<Customer> GetTestCustomers()
        {
            var customers = new List<Customer>
            {
                new Customer{
                    Id=1,
                    UserId=1,
                    User=new User{
                        Id=1,
                        Phone="111314181",
                        Email="gdfg@example.com",
                        Login="Vasya",
                        PasswordHash="12345",
                        IsActive=true }
                },
                new Customer{
                    Id=3,
                    UserId=2,
                    User=new User{
                        Id=2,
                        Phone="111314181",
                        Email="gdfg@example.com",
                        Login="Vitaliy",
                        PasswordHash="12345",
                        IsActive=true }
                },

            };
            return customers;
        }
        public static IList<Customer> GetTestCustomersForRole()
        {
            var customers = new List<Customer>
            {
                new Customer
                {
                    Id=1,
                    UserId=2
                }

            };
            return customers;
        }
        public static IList<Worker> GetTestWorkers()
        {
            var workers = new List<Worker>()
            {
                new Worker()
                {
                    Id=1,
                    UserId=3
                }
            };
            return workers;
        }
        public static IList<Customer> GetFullCustomersList()
        {

            var customers = new List<Customer>()
            {
                new Customer()
                {
                    Id=1,
                    UserId=2,
                    User = GetTestUsers()[1]
                },
                new Customer()
                {
                    Id = 2,
                    UserId = 1,
                    User = GetTestUsers()[0]
                },
                new Customer()
                {
                    Id = 3,
                    UserId = 3,
                    User = GetTestUsers()[2]
                }
             };
            return customers;


        }
        public static IList<Account> GetTestAccounts()
        {
            var accounts = new List<Account>
            {
                new Account
                {
                    Id=1,
                    CustomerId=1,
                    Customer=new Customer
                    {
                            Id=1,
                            UserId=1,
                            User=new User
                            {
                                Id=1,
                                Phone="14631161",
                                Email="example@gmail.com",
                                Login="Vasya",
                                PasswordHash="passwordHash",
                                IsActive=true
                            }
                    },
                    CurrencyCode="UAN",
                    Currency=new Domain.Models.Currency
                    {
                        Code="UAN",
                        Symbol="$"
                    },
                    Amount=1000
                },
                new Account
                {
                    Id=2,
                    CustomerId=1,
                    Customer=new Customer
                    {
                            Id=1,
                            UserId=1,
                            User=new User
                            {
                                Id=1,
                                Phone="14631161",
                                Email="example@gmail.com",
                                Login="Vasya",
                                PasswordHash="passwordHash",
                                IsActive=true
                            }
                    },
                    CurrencyCode="USD",
                    Currency=new Domain.Models.Currency
                    {
                        Code="USD",
                        Symbol="$"
                    },
                    Amount=1000
                },
                new Account
                {
                    Id=3,
                    CustomerId=3,
                    Customer=new Customer
                    {
                            Id=3,
                            UserId=3,
                            User=new User
                            {
                                Id=3,
                                Phone="14631161",
                                Email="example@gmail.com",
                                Login="Vitaliy",
                                PasswordHash="passwordHash",
                                IsActive=true
                            }
                    },
                    CurrencyCode="UAN",
                    Currency=new Domain.Models.Currency
                    {
                        Code="UAN",
                        Symbol="$"
                    },
                    Amount=1000
                }

            };

            return accounts;
        }
        public static IList<Account> GetTestAccountsList()
        {
            var accounts = new List<Account>
            {
                new Account
                {
                    Id=1,
                    CustomerId=1,
                    Customer=new Customer
                    {
                            Id=1,
                            UserId=1,
                            User=new User
                            {
                                Id=1,
                                Phone="14631161",
                                Email="example@gmail.com",
                                Login="Vasya",
                                PasswordHash="passwordHash",
                                IsActive=true
                            }
                    },
                    CurrencyCode="UAN",
                    Currency=new Domain.Models.Currency
                    {
                        Code="UAN",
                        Symbol="$"
                    },
                    Amount=1000
                },
                new Account
                {
                    Id=2,
                    CustomerId=1,
                    Customer=new Customer
                    {
                            Id=1,
                            UserId=1,
                            User=new User
                            {
                                Id=1,
                                Phone="14631161",
                                Email="example@gmail.com",
                                Login="Vasya",
                                PasswordHash="passwordHash",
                                IsActive=true
                            }
                    },
                    CurrencyCode="USD",
                    Currency=new Domain.Models.Currency
                    {
                        Code="USD",
                        Symbol="$"
                    },
                    Amount=1000
                },
                new Account
                {
                    Id=3,
                    CustomerId=3,
                    Customer=new Customer
                    {
                            Id=3,
                            UserId=3,
                            User=new User
                            {
                                Id=3,
                                Phone="14631161",
                                Email="example@gmail.com",
                                Login="Vitaliy",
                                PasswordHash="passwordHash",
                                IsActive=true
                            }
                    },
                    CurrencyCode="UAN",
                    Currency=new Domain.Models.Currency
                    {
                        Code="UAN",
                        Symbol="$"
                    },
                    Amount=1000
                }

            };

            return accounts;
        }

    }
}
