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
    public class CurrencyServiceTest
    {
        [Fact]
        public void GetAllCurrenciesServiceTest()
        {
            var mockSet = new Mock<DbSet<Currency>>();
            var testCurrencies = TestData.GetTestCurrencies();

            mockSet.As<IQueryable<Currency>>().Setup(m => m.Provider).Returns(testCurrencies.AsQueryable().Provider);
            mockSet.As<IQueryable<Currency>>().Setup(m => m.Expression).Returns(testCurrencies.AsQueryable().Expression);
            mockSet.As<IQueryable<Currency>>().Setup(m => m.ElementType).Returns(testCurrencies.AsQueryable().ElementType);
            mockSet.As<IQueryable<Currency>>().Setup(con => con.GetEnumerator()).Returns(testCurrencies.GetEnumerator());

            var mockContext = new Mock<ApplicationContext>();
            mockContext.Setup(con => con.Currency).Returns(mockSet.Object);

            ICurrencyService accountService = new CurrencyService(mockContext.Object);

            var currencies = accountService.GetAllCurrenciesService() as IList<Currency>;

            Assert.Equal(3, currencies.Count);
            Assert.Equal("UAN", currencies[0].Code);
            Assert.Equal("USD", currencies[1].Code);
            Assert.Equal("EUR", currencies[2].Code);
        }
    }
}
