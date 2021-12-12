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
    public class CustomerServiceTest
    {
        [Fact]
        public void GetCustomerIdFromLoginTest()
        {
            //Arrange

            var mockSet = new Mock<DbSet<Customer>>();

            var testCustomers = TestData.GetTestCustomers();


            mockSet.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(testCustomers.AsQueryable().Provider);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(testCustomers.AsQueryable().Expression);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(testCustomers.AsQueryable().ElementType);
            mockSet.As<IQueryable<Customer>>().Setup(con => con.GetEnumerator()).Returns(testCustomers.GetEnumerator());


            var mockContext = new Mock<ApplicationContext>();
            mockContext
                .Setup(con => con.Customers)
                .Returns(mockSet.Object);
            ICustomerService customerService = new CustomerService(mockContext.Object);

            //Act
            var customerId = customerService.GetCustomerIdFromLogin("Vitaliy");

            //Assert
            Assert.Equal(3, customerId);
        }
        [Fact]
        public void GetPaginatedUsersListOfCustomersTest()
        {
            #region arrange
            //Users
            var mockSetUser = new Mock<DbSet<User>>();

            var testUsers = TestData.GetTestUsers();

            mockSetUser.As<IQueryable<User>>().Setup(m => m.Provider).Returns(testUsers.AsQueryable().Provider);
            mockSetUser.As<IQueryable<User>>().Setup(m => m.Expression).Returns(testUsers.AsQueryable().Expression);
            mockSetUser.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(testUsers.AsQueryable().ElementType);
            mockSetUser.As<IQueryable<User>>().Setup(con => con.GetEnumerator()).Returns(testUsers.GetEnumerator());
            //Customers
            var mockSetCustomers = new Mock<DbSet<Customer>>();

            var testCustomers = TestData.GetFullCustomersList();

            mockSetCustomers.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(testCustomers.AsQueryable().Provider);
            mockSetCustomers.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(testCustomers.AsQueryable().Expression);
            mockSetCustomers.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(testCustomers.AsQueryable().ElementType);
            mockSetCustomers.As<IQueryable<Customer>>().Setup(con => con.GetEnumerator()).Returns(testCustomers.GetEnumerator());

            var mockContext = new Mock<ApplicationContext>();

            mockContext.Setup(id => id.Users).Returns(mockSetUser.Object);
            mockContext.Setup(id => id.Customers).Returns(mockSetCustomers.Object);

            ICustomerService customerService = new CustomerService(mockContext.Object);
            #endregion
            #region act

            var firstUser = (customerService.GetPaginatedUsersListOfCustomers(0, 1) as IList<Customer>)[0];
            var secondUser = (customerService.GetPaginatedUsersListOfCustomers(1, 1) as IList<Customer>)[0];
            var thirdUser = (customerService.GetPaginatedUsersListOfCustomers(2, 1) as IList<Customer>)[0];

            #endregion
            #region assert

            Assert.Equal("Vitaliy", firstUser.User.Login);
            Assert.Equal("Vasya", secondUser.User.Login);
            Assert.Equal("Nikita", thirdUser.User.Login);
            #endregion

        }
        [Fact]
        public void CustomerExistTest()
        {
            #region arrange
            //Users
            var mockSetUser = new Mock<DbSet<User>>();

            var testUsers = TestData.GetTestUsers();

            mockSetUser.As<IQueryable<User>>().Setup(m => m.Provider).Returns(testUsers.AsQueryable().Provider);
            mockSetUser.As<IQueryable<User>>().Setup(m => m.Expression).Returns(testUsers.AsQueryable().Expression);
            mockSetUser.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(testUsers.AsQueryable().ElementType);
            mockSetUser.As<IQueryable<User>>().Setup(con => con.GetEnumerator()).Returns(testUsers.GetEnumerator());
            //Customers
            var mockSetCustomers = new Mock<DbSet<Customer>>();

            var testCustomers = TestData.GetFullCustomersList();

            mockSetCustomers.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(testCustomers.AsQueryable().Provider);
            mockSetCustomers.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(testCustomers.AsQueryable().Expression);
            mockSetCustomers.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(testCustomers.AsQueryable().ElementType);
            mockSetCustomers.As<IQueryable<Customer>>().Setup(con => con.GetEnumerator()).Returns(testCustomers.GetEnumerator());

            var mockContext = new Mock<ApplicationContext>();

            mockContext.Setup(id => id.Users).Returns(mockSetUser.Object);
            mockContext.Setup(id => id.Customers).Returns(mockSetCustomers.Object);

            ICustomerService customerService = new CustomerService(mockContext.Object);
            #endregion
            #region act
            bool possible1 = customerService.CustomerExist(1);
            bool possible2 = customerService.CustomerExist(3);
            bool impossible = customerService.CustomerExist(10);
            #endregion
            #region assert
            Assert.True(possible1);
            Assert.True(possible2);
            Assert.False(impossible);
            #endregion
        }
    }
}
