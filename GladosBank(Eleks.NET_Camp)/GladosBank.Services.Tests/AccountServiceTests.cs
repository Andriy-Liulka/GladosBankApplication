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
        public void GetAllUserAccountsTest()
        {
            //Accounts 
            var mockSet = new Mock<DbSet<Account>>();
            var testAccounts = TestData.GetTestAccounts();

            mockSet.As<IQueryable<Account>>().Setup(m => m.Provider).Returns(testAccounts.AsQueryable().Provider);
            mockSet.As<IQueryable<Account>>().Setup(m => m.Expression).Returns(testAccounts.AsQueryable().Expression);
            mockSet.As<IQueryable<Account>>().Setup(m => m.ElementType).Returns(testAccounts.AsQueryable().ElementType);
            mockSet.As<IQueryable<Account>>().Setup(con => con.GetEnumerator()).Returns(testAccounts.GetEnumerator());

            //Customers
            var mockSet2 = new Mock<DbSet<Customer>>();
            var testCustomers = TestData.GetTestCustomers();

            mockSet2.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(testCustomers.AsQueryable().Provider);
            mockSet2.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(testCustomers.AsQueryable().Expression);
            mockSet2.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(testCustomers.AsQueryable().ElementType);
            mockSet2.As<IQueryable<Customer>>().Setup(con => con.GetEnumerator()).Returns(testCustomers.AsQueryable().GetEnumerator());

            //Test
            var mockContext = new Mock<ApplicationContext>();
            mockContext.Setup(con => con.Accounts).Returns(mockSet.Object);
            mockContext.Setup(con => con.Customers).Returns(mockSet2.Object);

            var customerService = new CustomerService(mockContext.Object);
            IAccountService accountService = new AccountService(mockContext.Object, null, customerService);

            var accounts = accountService.GetAllUserAccounts("Vasya");

            var number = (accounts as ICollection<Account>).Count;
            Assert.Equal(2,number);
            Assert.Equal("UAN", (accounts as IList<Account>)[0].CurrencyCode);
            Assert.Equal("USD", (accounts as IList<Account>)[1].CurrencyCode);
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

            IAccountService accountService = new AccountService(mockContext.Object, null,null);

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
            var testAccounts = TestData.GetTestAccounts();
            mockSet.As<IQueryable<Account>>().Setup(m => m.Provider).Returns(testAccounts.AsQueryable().Provider);
            mockSet.As<IQueryable<Account>>().Setup(m => m.Expression).Returns(testAccounts.AsQueryable().Expression);
            mockSet.As<IQueryable<Account>>().Setup(m => m.ElementType).Returns(testAccounts.AsQueryable().ElementType);
            mockSet.As<IQueryable<Account>>().Setup(con => con.GetEnumerator()).Returns(testAccounts.GetEnumerator());

            var mockContext = new Mock<ApplicationContext>();
            mockContext.Setup(acc => acc.Accounts).Returns(mockSet.Object);

            IAccountService accountService = new AccountService(mockContext.Object, null,null);

            var replenichedAccountId=accountService.ReplenishAccount(1,100);
            mockSet.Verify(m => m.Update(It.IsAny<Account>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());

            Assert.Equal(1100, mockContext.Object.Accounts.FirstOrDefault(id=>id.Id.Equals(replenichedAccountId)).Amount);
        }
        [Fact]
        public void DeleteAccountTest()
        {
            var mockSet = new Mock<DbSet<Account>>();
            var testAccounts = TestData.GetTestAccountsList();
            mockSet.As<IQueryable<Account>>().Setup(m => m.Provider).Returns(testAccounts.AsQueryable().AsQueryable().Provider);
            mockSet.As<IQueryable<Account>>().Setup(m => m.Expression).Returns(testAccounts.AsQueryable().AsQueryable().Expression);
            mockSet.As<IQueryable<Account>>().Setup(m => m.ElementType).Returns(testAccounts.AsQueryable().AsQueryable().ElementType);
            mockSet.As<IQueryable<Account>>().Setup(con => con.GetEnumerator()).Returns(testAccounts.GetEnumerator());

            var mockContext = new Mock<ApplicationContext>();
            mockContext.Setup(acc => acc.Accounts).Returns(mockSet.Object);
            mockSet.Setup(m => m.Remove(It.IsAny<Account>())).Callback<Account>((entity) => testAccounts.Remove(entity));

            IAccountService accountService = new AccountService(mockContext.Object, null,null);
            
            var deletedAccountId = accountService.DeleteAccount(1);
            mockSet.Verify(m => m.Remove(It.IsAny<Account>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());

            Assert.False(mockContext.Object.Accounts.Any(acc => acc.Id.Equals(deletedAccountId)));
        }
    }
}
