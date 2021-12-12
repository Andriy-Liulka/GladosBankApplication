using GladosBank.Domain;
using GladosBank.Services.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GladosBank.Services.Tests
{
    public class TransferMoneyTest
    {
        [Fact]
        public void TransferMoneySuccessfullyTest()
        {
            var account1 = new Account
            {
                Id = 1,
                CustomerId = 1,
                Customer = new Customer
                {
                    Id = 1,
                    UserId = 1,
                    User = new User
                    {
                        Id = 1,
                        Phone = "14631161",
                        Email = "example@gmail.com",
                        Login = "Vasya",
                        PasswordHash = "passwordHash",
                        IsActive = true
                    }
                },
                CurrencyCode = "UAN",
                Currency = new Domain.Models.Currency
                {
                    Code = "UAN",
                    Symbol = "$"
                },
                Amount = 10000
            };
            var account2 = new Account
            {
                Id = 2,
                CustomerId = 2,
                Customer = new Customer
                {
                    Id = 2,
                    UserId = 2,
                    User = new User
                    {
                        Id = 2,
                        Phone = "16164161",
                        Email = "example2@gmail.com",
                        Login = "Vitaliy",
                        PasswordHash = "passwordHash1",
                        IsActive = true
                    }
                },
                CurrencyCode = "UAN",
                Currency = new Domain.Models.Currency
                {
                    Code = "UAN",
                    Symbol = "$"
                },
                Amount = 10000
            };

            IAccountService accountService = new AccountService(null,null,null);

            accountService.TransferMoney(2000, account1, account2);

            Assert.Equal(8000,account1.Amount);
            Assert.Equal(12000, account2.Amount);
        }
        [Fact]
        public void TransferMoneyArgumentNullExceptionTest()
        {
            Account account1 = null;
            Account account2 = null;

            IAccountService accountService = new AccountService(null, null, null);

            Assert.Throws<ArgumentNullException>(() => accountService.TransferMoney(2000, account1, account2));
        }
        [Fact]
        public void TransferMoneyArgumentOutOfRangeExceptionTest()
        {
            var account1 = new Account
            {
                Id = 1,
                CustomerId = 1,
                Customer = new Customer
                {
                    Id = 1,
                    UserId = 1,
                    User = new User
                    {
                        Id = 1,
                        Phone = "14631161",
                        Email = "example@gmail.com",
                        Login = "Vasya",
                        PasswordHash = "passwordHash",
                        IsActive = true
                    }
                },
                CurrencyCode = "UAN",
                Currency = new Domain.Models.Currency
                {
                    Code = "UAN",
                    Symbol = "$"
                },
                Amount = 10000
            };
            var account2 = new Account
            {
                Id = 2,
                CustomerId = 2,
                Customer = new Customer
                {
                    Id = 2,
                    UserId = 2,
                    User = new User
                    {
                        Id = 2,
                        Phone = "16164161",
                        Email = "example2@gmail.com",
                        Login = "Vitaliy",
                        PasswordHash = "passwordHash1",
                        IsActive = true
                    }
                },
                CurrencyCode = "UAN",
                Currency = new Domain.Models.Currency
                {
                    Code = "UAN",
                    Symbol = "$"
                },
                Amount = 10000
            };

            IAccountService accountService = new AccountService(null, null, null);

            Assert.Throws<ArgumentOutOfRangeException>(() => accountService.TransferMoney(-1000, account1, account2));
            Assert.Throws<ArgumentOutOfRangeException>(() => accountService.TransferMoney(0, account1, account2));
        }
        [Fact]
        public void TransferMoneyDifferentCurrencyExceptionTest()
        {
            var account1 = new Account
            {
                Id = 1,
                CustomerId = 1,
                Customer = new Customer
                {
                    Id = 1,
                    UserId = 1,
                    User = new User
                    {
                        Id = 1,
                        Phone = "14631161",
                        Email = "example@gmail.com",
                        Login = "Vasya",
                        PasswordHash = "passwordHash",
                        IsActive = true
                    }
                },
                CurrencyCode = "UAN",
                Currency = new Domain.Models.Currency
                {
                    Code = "UAN",
                    Symbol = "$"
                },
                Amount = 10000
            };
            var account2 = new Account
            {
                Id = 2,
                CustomerId = 2,
                Customer = new Customer
                {
                    Id = 2,
                    UserId = 2,
                    User = new User
                    {
                        Id = 2,
                        Phone = "16164161",
                        Email = "example2@gmail.com",
                        Login = "Vitaliy",
                        PasswordHash = "passwordHash1",
                        IsActive = true
                    }
                },
                CurrencyCode = "EUR",
                Currency = new Domain.Models.Currency
                {
                    Code = "UAN",
                    Symbol = "$"
                },
                Amount = 10000
            };

            IAccountService accountService = new AccountService(null, null, null);

            Assert.Throws<DifferentCurrencyException>(() => accountService.TransferMoney(1000, account1, account2));
        }
        [Fact]
        public void TransferMoneyTooLittleAccountAmountExceptionTest()
        {
            var account1 = new Account
            {
                Id = 1,
                CustomerId = 1,
                Customer = new Customer
                {
                    Id = 1,
                    UserId = 1,
                    User = new User
                    {
                        Id = 1,
                        Phone = "14631161",
                        Email = "example@gmail.com",
                        Login = "Vasya",
                        PasswordHash = "passwordHash",
                        IsActive = true
                    }
                },
                CurrencyCode = "UAN",
                Currency = new Domain.Models.Currency
                {
                    Code = "UAN",
                    Symbol = "$"
                },
                Amount = 10000
            };
            var account2 = new Account
            {
                Id = 2,
                CustomerId = 2,
                Customer = new Customer
                {
                    Id = 2,
                    UserId = 2,
                    User = new User
                    {
                        Id = 2,
                        Phone = "16164161",
                        Email = "example2@gmail.com",
                        Login = "Vitaliy",
                        PasswordHash = "passwordHash1",
                        IsActive = true
                    }
                },
                CurrencyCode = "UAN",
                Currency = new Domain.Models.Currency
                {
                    Code = "UAN",
                    Symbol = "$"
                },
                Amount = 10000
            };

            IAccountService accountService = new AccountService(null, null, null);

            Assert.Throws<TooLittleAccountAmountException>(() => accountService.TransferMoney(1000000, account1, account2));
        }      
        [Fact]
        public void TransferMoneyTheSameAccountExceptionTest()
        {
            var account1 = new Account
            {
                Id = 1,
                CustomerId = 1,
                Customer = new Customer
                {
                    Id = 1,
                    UserId = 1,
                    User = new User
                    {
                        Id = 1,
                        Phone = "14631161",
                        Email = "example@gmail.com",
                        Login = "Vasya",
                        PasswordHash = "passwordHash",
                        IsActive = true
                    }
                },
                CurrencyCode = "UAN",
                Currency = new Domain.Models.Currency
                {
                    Code = "UAN",
                    Symbol = "$"
                },
                Amount = 10000
            };

            IAccountService accountService = new AccountService(null, null, null);

            Assert.Throws<TheSameAccountException>(() => accountService.TransferMoney(100, account1, account1));
        }
    }
}
