using GladosBank.Domain;
using GladosBank.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GladosBank.Services.Tests
{
    public class AccountServiceTests
    {
        [Fact]
        public void GetCustomerIdFromLoginTest()
        {
            //Arrange

            var mockSet = new Mock<DbSet<Customer>>();

            var testCustomers = GetTestCustomers();


            mockSet.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(testCustomers.Provider);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(testCustomers.Expression);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(testCustomers.ElementType);
            mockSet.As<IQueryable<Customer>>().Setup(con => con.GetEnumerator()).Returns(testCustomers.GetEnumerator());


            var mockContext = new Mock<ApplicationContext>();
            mockContext
                .Setup(con => con.Customers)
                .Returns(mockSet.Object);
            #region ExtraCodeToUserService
            //var mockSet2 = new Mock<DbSet<User>>();

            //mockSet2.As<IQueryable<User>>()
            //    .Setup(con => con.GetEnumerator())
            //    .Returns(GetTestUsers().GetEnumerator());

            //var mockContext2 = new Mock<ApplicationContext>();
            //mockContext2
            //    .Setup(con => con.Users)
            //    .Returns(mockSet2.Object);
            //var userService = new UserService(mockContext2.Object);
            #endregion
            IAccountService accountService = new AccountService(mockContext.Object, null);

            //Act
            var customerId=accountService.GetCustomerIdFromLogin("Vitaliy");

            //Assert
            Assert.Equal(3, customerId);
        }
        [Fact]
        public void GetAllUserAccountsTest()
        {
            //Accounts 
            var mockSet = new Mock<DbSet<Account>>();
            var testAccounts = GetTestAccounts();

            mockSet.As<IQueryable<Account>>().Setup(m => m.Provider).Returns(testAccounts.Provider);
            mockSet.As<IQueryable<Account>>().Setup(m => m.Expression).Returns(testAccounts.Expression);
            mockSet.As<IQueryable<Account>>().Setup(m => m.ElementType).Returns(testAccounts.ElementType);
            mockSet.As<IQueryable<Account>>().Setup(con => con.GetEnumerator()).Returns(testAccounts.GetEnumerator());

            //Customers
            var mockSet2 = new Mock<DbSet<Customer>>();
            var testCustomers = GetTestCustomers();

            mockSet2.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(testCustomers.Provider);
            mockSet2.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(testCustomers.Expression);
            mockSet2.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(testCustomers.ElementType);
            mockSet2.As<IQueryable<Customer>>().Setup(con => con.GetEnumerator()).Returns(testCustomers.GetEnumerator());

            //Test
            var mockContext = new Mock<ApplicationContext>();
            mockContext.Setup(con => con.Accounts).Returns(mockSet.Object);
            mockContext.Setup(con => con.Customers).Returns(mockSet2.Object);

            IAccountService accountService = new AccountService(mockContext.Object, null);

            var accounts = accountService.GetAllUserAccounts("Vasya");

            var number = (accounts as ICollection<Account>).Count;
            Assert.Equal(2,number);
            Assert.Equal("UAN", (accounts as IList<Account>)[0].CurrencyCode);
            Assert.Equal("USD", (accounts as IList<Account>)[1].CurrencyCode);
        }
        [Fact]
        public void GetAllCurrenciesServiceTest()
        {
            var mockSet = new Mock<DbSet<Currency>>();
            var testCurrencies = GetTestCurrencies();

            mockSet.As<IQueryable<Currency>>().Setup(m => m.Provider).Returns(testCurrencies.Provider);
            mockSet.As<IQueryable<Currency>>().Setup(m => m.Expression).Returns(testCurrencies.Expression);
            mockSet.As<IQueryable<Currency>>().Setup(m => m.ElementType).Returns(testCurrencies.ElementType);
            mockSet.As<IQueryable<Currency>>().Setup(con => con.GetEnumerator()).Returns(testCurrencies.GetEnumerator());

            var mockContext = new Mock<ApplicationContext>();
            mockContext.Setup(con => con.Currency).Returns(mockSet.Object);

            IAccountService accountService = new AccountService(mockContext.Object, null);

            var currencies = accountService.GetAllCurrenciesService() as IList<Currency>;

            Assert.Equal(3, currencies.Count);
            Assert.Equal("UAN", currencies[0].Code);
            Assert.Equal("USD", currencies[1].Code);
            Assert.Equal("EUR", currencies[2].Code);
        }
        [Fact]
        public void CreateAccountTest()
        {
            #region Arrange

            var mockSet = new Mock<DbSet<Account>>();
            IList<Account> testAccounts = new List<Account> { new Account {Id=50,CurrencyCode="EUR" } };
            mockSet.As<IQueryable<Account>>().Setup(m => m.Provider).Returns(testAccounts.AsQueryable().Provider);
            mockSet.As<IQueryable<Account>>().Setup(m => m.Expression).Returns(testAccounts.AsQueryable().Expression);
            mockSet.As<IQueryable<Account>>().Setup(m => m.ElementType).Returns(testAccounts.AsQueryable().ElementType);
            mockSet.As<IQueryable<Account>>().Setup(con => con.GetEnumerator()).Returns(testAccounts.GetEnumerator());

            var mockSet2 = new Mock<DbSet<Customer>>();
            IQueryable<Customer> testCustomers = new List<Customer>{new Customer { Id = 1 }}.AsQueryable();
            mockSet2.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(testCustomers.Provider);
            mockSet2.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(testCustomers.Expression);
            mockSet2.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(testCustomers.ElementType);
            mockSet2.As<IQueryable<Customer>>().Setup(con => con.GetEnumerator()).Returns(testCustomers.GetEnumerator());

            var mockSet3 = new Mock<DbSet<Currency>>();
            IQueryable<Currency> testCurrencies = new List<Currency> { new Currency {Code= "UAN",Symbol= "₴" } }.AsQueryable();
            mockSet3.As<IQueryable<Currency>>().Setup(m => m.Provider).Returns(testCurrencies.Provider);
            mockSet3.As<IQueryable<Currency>>().Setup(m => m.Expression).Returns(testCurrencies.Expression);
            mockSet3.As<IQueryable<Currency>>().Setup(m => m.ElementType).Returns(testCurrencies.ElementType);
            mockSet3.As<IQueryable<Currency>>().Setup(con => con.GetEnumerator()).Returns(testCurrencies.GetEnumerator());

            var mockContext = new Mock<ApplicationContext>();
            mockContext.Setup(acc => acc.Accounts).Returns(mockSet.Object);
            mockContext.Setup(acc => acc.Customers).Returns(mockSet2.Object);
            mockContext.Setup(acc => acc.Currency).Returns(mockSet3.Object);
            mockSet.Setup(m => m.Add(It.IsAny<Account>())).Callback<Account>((entity) => testAccounts.Add(entity));

            IAccountService accountService = new AccountService(mockContext.Object, null);

            var tectAccount = new Account
            {
                Id = 1,
                CurrencyCode = "UAN",
                Currency = new Currency
                {
                    Code = "UAN",
                    Symbol = "₴"
                },
                Amount = 1000,
                CustomerId = 1,
                DateOfCreating = DateTime.UtcNow,
                Notes="Test account"
            };
            #endregion
            #region Act

            var existingAccountId = accountService.CreateAccount(tectAccount);
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
            
            #endregion
            #region Assert

            Assert.Equal(tectAccount.Id, existingAccountId);
            Assert.True(mockContext.Object.Accounts.Any(acc=>acc.Id.Equals(existingAccountId)));
            #endregion
        }
        [Fact]
        public void ReplenishAccountTest()
        {
            var mockSet = new Mock<DbSet<Account>>();
            var testAccounts = GetTestAccounts();
            mockSet.As<IQueryable<Account>>().Setup(m => m.Provider).Returns(testAccounts.Provider);
            mockSet.As<IQueryable<Account>>().Setup(m => m.Expression).Returns(testAccounts.Expression);
            mockSet.As<IQueryable<Account>>().Setup(m => m.ElementType).Returns(testAccounts.ElementType);
            mockSet.As<IQueryable<Account>>().Setup(con => con.GetEnumerator()).Returns(testAccounts.GetEnumerator());

            var mockContext = new Mock<ApplicationContext>();
            mockContext.Setup(acc => acc.Accounts).Returns(mockSet.Object);

            IAccountService accountService = new AccountService(mockContext.Object, null);

            var replenichedAccountId=accountService.ReplenishAccount(1,100);
            mockSet.Verify(m => m.Update(It.IsAny<Account>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());

            Assert.Equal(1100, mockContext.Object.Accounts.FirstOrDefault(id=>id.Id.Equals(replenichedAccountId)).Amount);
        }
        [Fact]
        public void DeleteAccountTest()
        {
            var mockSet = new Mock<DbSet<Account>>();
            var testAccounts = GetTestAccountsList();
            mockSet.As<IQueryable<Account>>().Setup(m => m.Provider).Returns(testAccounts.AsQueryable().Provider);
            mockSet.As<IQueryable<Account>>().Setup(m => m.Expression).Returns(testAccounts.AsQueryable().Expression);
            mockSet.As<IQueryable<Account>>().Setup(m => m.ElementType).Returns(testAccounts.AsQueryable().ElementType);
            mockSet.As<IQueryable<Account>>().Setup(con => con.GetEnumerator()).Returns(testAccounts.GetEnumerator());

            var mockContext = new Mock<ApplicationContext>();
            mockContext.Setup(acc => acc.Accounts).Returns(mockSet.Object);
            mockSet.Setup(m => m.Remove(It.IsAny<Account>())).Callback<Account>((entity) => testAccounts.Remove(entity));

            IAccountService accountService = new AccountService(mockContext.Object, null);
            
            var deletedAccountId = accountService.DeleteAccount(1);
            mockSet.Verify(m => m.Remove(It.IsAny<Account>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());

            Assert.False(mockContext.Object.Accounts.Any(acc => acc.Id.Equals(deletedAccountId)));
        }
        #region NotTestMethods
        private IQueryable<Customer> GetTestCustomers()
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

            }.AsQueryable();
            return customers;
        }

        private IQueryable<User> GetTestUsers()
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
                        IsActive=true }

            }.AsQueryable();
            return users;
        }

        private IQueryable<Account> GetTestAccounts()
        {
            var accounts = new  List<Account>
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

            }.AsQueryable();

            return accounts;
        }

        private IList<Account> GetTestAccountsList()
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

        private IQueryable<Currency> GetTestCurrencies()
        {
            var currenies = new List<Currency>
            {
                new Currency{Code="UAN",Symbol="₴" },
                new Currency{Code="USD",Symbol="$" },
                new Currency{Code="EUR",Symbol="€" }
            }.AsQueryable();
            return currenies;
        }
        #endregion
    }
}
