using GladosBank.Domain;
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
    public class OperationHistoryServiceTest
    {
        [Fact]
        public void KeepHistoryElementOfOperationTest()
        {
            #region arrange
            //Operation Histories
            var mockOperationHistoriesSet = new Mock<DbSet<OperationsHistory>>();

            var testOperationHistories = new List<OperationsHistory>();

            mockOperationHistoriesSet.As<IQueryable<OperationsHistory>>().Setup(m => m.Provider).Returns(testOperationHistories.AsQueryable().Provider);
            mockOperationHistoriesSet.As<IQueryable<OperationsHistory>>().Setup(m => m.Expression).Returns(testOperationHistories.AsQueryable().Expression);
            mockOperationHistoriesSet.As<IQueryable<OperationsHistory>>().Setup(m => m.ElementType).Returns(testOperationHistories.AsQueryable().ElementType);
            mockOperationHistoriesSet.As<IQueryable<OperationsHistory>>().Setup(con => con.GetEnumerator()).Returns(testOperationHistories.GetEnumerator());
            mockOperationHistoriesSet.Setup(m => m.Add(It.IsAny<OperationsHistory>())).Callback<OperationsHistory>((entity) => testOperationHistories.Add(entity));

            //Customers
            var mockCustomersSet = new Mock<DbSet<Customer>>();

            var testCustomers = new List<Customer>()
            {
                new Customer
                {
                    Id=1,
                    UserId=1
                }
            };

            mockCustomersSet.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(testCustomers.AsQueryable().Provider);
            mockCustomersSet.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(testCustomers.AsQueryable().Expression);
            mockCustomersSet.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(testCustomers.AsQueryable().ElementType);
            mockCustomersSet.As<IQueryable<Customer>>().Setup(con => con.GetEnumerator()).Returns(testCustomers.GetEnumerator());

            var mockContext = new Mock<ApplicationContext>();

            mockContext
                .Setup(us => us.OperationsHistory)
                .Returns(mockOperationHistoriesSet.Object);
            mockContext
                .Setup(us => us.Customers)
                .Returns(mockCustomersSet.Object);

            var customerService = new CustomerService(mockContext.Object);
            IOperationHistoryService operationHistoryService = new OperationHistoryService(mockContext.Object, customerService);

            var testOperationHistory = new OperationsHistory
            {
                Id = 1,
                CustomerId = 1,
                DateTime = DateTime.UtcNow,
                Description = "Test operation history"
            };
            #endregion
            #region act
            operationHistoryService.KeepHistoryElementOfOperation(testOperationHistory);
            #endregion
            #region assert
            Assert.True(mockContext.Object.OperationsHistory.Any(p => p.Id.Equals(testOperationHistory.Id)));
            Assert.Equal(1, mockContext.Object.OperationsHistory.Count());
            #endregion
        }
    }
}
