using GladosBank.Domain;
using GladosBank.Services.Exceptions;
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
    public class UserServiceTests
    {
        [Fact]
        public void IsSuchLoginInDatabaseTest()
        {
            #region arrange
            var mockSet = new Mock<DbSet<User>>();

            var testUsers = GetTestUsers();

            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(testUsers.AsQueryable().Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(testUsers.AsQueryable().Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(testUsers.AsQueryable().ElementType);
            mockSet.As<IQueryable<User>>().Setup(con => con.GetEnumerator()).Returns(testUsers.GetEnumerator());

            var mockContext = new Mock<ApplicationContext>();

            mockContext
                .Setup(id => id.Users)
                .Returns(mockSet.Object);

            IUserService userService = new UserService(mockContext.Object);
            #endregion
            #region act
            bool existingUser=userService.IsSuchLoginInDatabase("Vasya");
            bool unexistingUser= userService.IsSuchLoginInDatabase("Oleh");
            #endregion
            #region assert
            Assert.True(existingUser);
            Assert.False(unexistingUser);
            #endregion
        }
        [Fact]
        public void SetRoleToSpecifiedUserTest()
        {
            #region arrange
            //Test Users
            var mockUserSet = new Mock<DbSet<User>>();

            var testUsers = GetTestUsers();

            mockUserSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(testUsers.AsQueryable().Provider);
            mockUserSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(testUsers.AsQueryable().Expression);
            mockUserSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(testUsers.AsQueryable().ElementType);
            mockUserSet.As<IQueryable<User>>().Setup(con => con.GetEnumerator()).Returns(testUsers.GetEnumerator());

            //Test Customers
            var mockCustomersSet = new Mock<DbSet<Customer>>();

            var testCustomers = new List<Customer>();

            mockCustomersSet.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(testCustomers.AsQueryable().Provider);
            mockCustomersSet.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(testCustomers.AsQueryable().Expression);
            mockCustomersSet.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(testCustomers.AsQueryable().ElementType);
            mockCustomersSet.As<IQueryable<Customer>>().Setup(con => con.GetEnumerator()).Returns(testCustomers.GetEnumerator());

            //Test Admins
            var mockAdminsSet = new Mock<DbSet<Admin>>();

            var testAdmins = new List<Admin>();

            mockAdminsSet.As<IQueryable<Admin>>().Setup(m => m.Provider).Returns(testAdmins.AsQueryable().Provider);
            mockAdminsSet.As<IQueryable<Admin>>().Setup(m => m.Expression).Returns(testAdmins.AsQueryable().Expression);
            mockAdminsSet.As<IQueryable<Admin>>().Setup(m => m.ElementType).Returns(testAdmins.AsQueryable().ElementType);
            mockAdminsSet.As<IQueryable<Admin>>().Setup(con => con.GetEnumerator()).Returns(testAdmins.GetEnumerator());


            //Test Workers
            var mockWorkersSet = new Mock<DbSet<Worker>>();

            var testWorkers = new List<Worker>();

            mockWorkersSet.As<IQueryable<Worker>>().Setup(m => m.Provider).Returns(testWorkers.AsQueryable().Provider);
            mockWorkersSet.As<IQueryable<Worker>>().Setup(m => m.Expression).Returns(testWorkers.AsQueryable().Expression);
            mockWorkersSet.As<IQueryable<Worker>>().Setup(m => m.ElementType).Returns(testWorkers.AsQueryable().ElementType);
            mockWorkersSet.As<IQueryable<Worker>>().Setup(con => con.GetEnumerator()).Returns(testWorkers.GetEnumerator());


            mockCustomersSet.Setup(m => m.Add(It.IsAny<Customer>())).Callback<Customer>((entity) => testCustomers.Add(entity));
            mockAdminsSet.Setup(m => m.Add(It.IsAny<Admin>())).Callback<Admin>((entity) => testAdmins.Add(entity));
            mockWorkersSet.Setup(m => m.Add(It.IsAny<Worker>())).Callback<Worker>((entity) => testWorkers.Add(entity));

            var mockContext = new Mock<ApplicationContext>();

            mockContext.Setup(id => id.Users).Returns(mockUserSet.Object);
            mockContext.Setup(id => id.Admins).Returns(mockAdminsSet.Object);
            mockContext.Setup(id => id.Workers).Returns(mockWorkersSet.Object);
            mockContext.Setup(id => id.Customers).Returns(mockCustomersSet.Object);

            #endregion
            #region act
            IUserService userService = new UserService(mockContext.Object);

            userService.SetRoleToSpecifiedUser(testUsers[0],"Customer");
            userService.SetRoleToSpecifiedUser(testUsers[1], "Admin");
            userService.SetRoleToSpecifiedUser(testUsers[2], "Worker");
            #endregion
            #region assert
            Assert.True(mockContext.Object.Customers.Any(us=>us.UserId.Equals(testUsers[0].Id)));
            Assert.True(mockContext.Object.Admins.Any(us => us.UserId.Equals(testUsers[1].Id)));
            Assert.True(mockContext.Object.Workers.Any(us => us.UserId.Equals(testUsers[2].Id)));
            #endregion
        }
        [Fact]
        public void CheckWhetherSuchUserExistTest()
        {
            #region arrange
            var mockUserSet = new Mock<DbSet<User>>();

            var testUsers = GetTestUsers();

            mockUserSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(testUsers.AsQueryable().Provider);
            mockUserSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(testUsers.AsQueryable().Expression);
            mockUserSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(testUsers.AsQueryable().ElementType);
            mockUserSet.As<IQueryable<User>>().Setup(con => con.GetEnumerator()).Returns(testUsers.GetEnumerator());

            var mockContext = new Mock<ApplicationContext>();

            mockContext
                .Setup(us => us.Users)
                .Returns(mockUserSet.Object);

            IUserService userService = new UserService(mockContext.Object);
            var testUser = new User
            {
                Id = 1,
                Phone = "111314181",
                Email = "gdfg@example.com",
                Login = "Vasya",
                PasswordHash = "12345",
                IsActive = true
            };
            #endregion
            #region act
            bool result = userService.CheckWhetherSuchUserExist(testUser);
            #endregion
            #region assert
            Assert.True(result);
            #endregion
        }
        [Fact]
        public void KeepHistoryElementOfOperationTest()
        {
            #region arrange
            //Operation Histories
            var mockOperationHistoriesSet = new Mock<DbSet<OperationsHistory>>();

            var testOperationHistories =new List<OperationsHistory>();

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

            IUserService userService = new UserService(mockContext.Object);

            var testOperationHistory = new OperationsHistory
            {
                Id = 1,
                CustomerId = 1,
                DateTime = DateTime.UtcNow,
                Description="Test operation history"
            };
            #endregion
            #region act
            userService.KeepHistoryElementOfOperation(testOperationHistory);
            #endregion
            #region assert
            Assert.True(mockContext.Object.OperationsHistory.Any(p=>p.Id.Equals(testOperationHistory.Id)));
            Assert.Equal(1,mockContext.Object.OperationsHistory.Count());
            #endregion
        }
        [Fact]
        public void DeleteUserTest()
        {
            #region arrange

            var mockSet = new Mock<DbSet<User>>();
            var testUsers = GetTestUsers();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(testUsers.AsQueryable().Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(testUsers.AsQueryable().Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(testUsers.AsQueryable().ElementType);
            mockSet.As<IQueryable<User>>().Setup(con => con.GetEnumerator()).Returns(testUsers.GetEnumerator());

            var mockContext = new Mock<ApplicationContext>();
            mockContext.Setup(acc => acc.Users).Returns(mockSet.Object);
            mockSet.Setup(m => m.Remove(It.IsAny<User>())).Callback<User>((entity) => testUsers.Remove(entity));

            IUserService userService = new UserService(mockContext.Object);

            var accountToDelete = new User
            {
                Id = 1,
                Phone = "111314181",
                Email = "gdfg@example.com",
                Login = "Vasya",
                PasswordHash = "12345",
                IsActive = true
            };
            #endregion
            #region act

            userService.DeleteUser(accountToDelete.Id);

            #endregion
            #region assert
            Assert.False(mockContext.Object.Users.Any(acc=>acc.Id.Equals(accountToDelete.Id)));
            #endregion
        }
        [Fact]
        public void GetUserByLoginTest()
        {
            #region arrange

            var mockSet = new Mock<DbSet<User>>();
            var testUsers = GetTestUsers();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(testUsers.AsQueryable().Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(testUsers.AsQueryable().Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(testUsers.AsQueryable().ElementType);
            mockSet.As<IQueryable<User>>().Setup(con => con.GetEnumerator()).Returns(testUsers.GetEnumerator());

            var mockContext = new Mock<ApplicationContext>();
            mockContext.Setup(acc => acc.Users).Returns(mockSet.Object);

            IUserService userService = new UserService(mockContext.Object);

            #endregion
            #region act

            var testUser = userService.GetUserByLogin("Vasya");

            #endregion
            #region assert

            Assert.Equal(1, testUser.Id);
            Assert.Equal("gdfg@example.com", testUser.Email);
            Assert.Equal("12345", testUser.PasswordHash);
            Assert.True(testUser.IsActive);
            #endregion

        }
        [Fact]
        public void GetRoleTest()
        {
            #region arrange
            //Users
            var mockSetUser = new Mock<DbSet<User>>();

            var testUsers = GetTestUsers();

            mockSetUser.As<IQueryable<User>>().Setup(m => m.Provider).Returns(testUsers.AsQueryable().Provider);
            mockSetUser.As<IQueryable<User>>().Setup(m => m.Expression).Returns(testUsers.AsQueryable().Expression);
            mockSetUser.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(testUsers.AsQueryable().ElementType);
            mockSetUser.As<IQueryable<User>>().Setup(con => con.GetEnumerator()).Returns(testUsers.GetEnumerator());

            //Admins
            var mockSetAdmins = new Mock<DbSet<Admin>>();

            var testAdmins = GetTestAdmins();

            mockSetAdmins.As<IQueryable<Admin>>().Setup(m => m.Provider).Returns(testAdmins.AsQueryable().Provider);
            mockSetAdmins.As<IQueryable<Admin>>().Setup(m => m.Expression).Returns(testAdmins.AsQueryable().Expression);
            mockSetAdmins.As<IQueryable<Admin>>().Setup(m => m.ElementType).Returns(testAdmins.AsQueryable().ElementType);
            mockSetAdmins.As<IQueryable<Admin>>().Setup(con => con.GetEnumerator()).Returns(testAdmins.GetEnumerator());

            //Workers
            var mockSetWorkers = new Mock<DbSet<Worker>>();

            var testWorkers = GetTestWorkers();

            mockSetWorkers.As<IQueryable<Worker>>().Setup(m => m.Provider).Returns(testWorkers.AsQueryable().Provider);
            mockSetWorkers.As<IQueryable<Worker>>().Setup(m => m.Expression).Returns(testWorkers.AsQueryable().Expression);
            mockSetWorkers.As<IQueryable<Worker>>().Setup(m => m.ElementType).Returns(testWorkers.AsQueryable().ElementType);
            mockSetWorkers.As<IQueryable<Worker>>().Setup(con => con.GetEnumerator()).Returns(testWorkers.GetEnumerator());

            //Customers
            var mockSetCustomers = new Mock<DbSet<Customer>>();

            var testCustomers = GetTestCustomers();

            mockSetCustomers.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(testCustomers.AsQueryable().Provider);
            mockSetCustomers.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(testCustomers.AsQueryable().Expression);
            mockSetCustomers.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(testCustomers.AsQueryable().ElementType);
            mockSetCustomers.As<IQueryable<Customer>>().Setup(con => con.GetEnumerator()).Returns(testCustomers.GetEnumerator());

            var mockContext = new Mock<ApplicationContext>();

            mockContext.Setup(id => id.Users).Returns(mockSetUser.Object);
            mockContext.Setup(id => id.Workers).Returns(mockSetWorkers.Object);
            mockContext.Setup(id => id.Customers).Returns(mockSetCustomers.Object);
            mockContext.Setup(id => id.Admins).Returns(mockSetAdmins.Object);

            IUserService userService = new UserService(mockContext.Object);

            #endregion
            #region act
            var adminRole=userService.GetRole("Vasya");
            var customerRole=userService.GetRole("Vitaliy");
            var workerRole=userService.GetRole("Nikita");
            #endregion
            #region assert
            Assert.Equal("Admin", adminRole);
            Assert.Equal("Customer", customerRole);
            Assert.Equal("Worker", workerRole);
            #endregion
        }
        [Fact]
        public void GetAllUsersTest()
        {
            #region arrange
            var mockSetUser = new Mock<DbSet<User>>();

            var testUsers = GetTestUsers();

            mockSetUser.As<IQueryable<User>>().Setup(m => m.Provider).Returns(testUsers.AsQueryable().Provider);
            mockSetUser.As<IQueryable<User>>().Setup(m => m.Expression).Returns(testUsers.AsQueryable().Expression);
            mockSetUser.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(testUsers.AsQueryable().ElementType);
            mockSetUser.As<IQueryable<User>>().Setup(con => con.GetEnumerator()).Returns(testUsers.GetEnumerator());

            var mockContext = new Mock<ApplicationContext>();

            mockContext.Setup(id => id.Users).Returns(mockSetUser.Object);

            IUserService userService = new UserService(mockContext.Object);
            #endregion
            #region act

            var usersList = userService.GetAllUsers();

            #endregion
            #region assert
            Assert.Equal(3,usersList.Count());
            Assert.Equal("Vasya", (usersList as IList<User>)[0].Login);
            Assert.Equal("Vitaliy", (usersList as IList<User>)[1].Login);
            Assert.Equal("Nikita", (usersList as IList<User>)[2].Login);
            #endregion
        }
        [Fact]
        public void GetPaginatedUsersListTest()
        {
            #region arrange
            var mockSetUser = new Mock<DbSet<User>>();

            var testUsers = GetTestUsers();

            mockSetUser.As<IQueryable<User>>().Setup(m => m.Provider).Returns(testUsers.AsQueryable().Provider);
            mockSetUser.As<IQueryable<User>>().Setup(m => m.Expression).Returns(testUsers.AsQueryable().Expression);
            mockSetUser.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(testUsers.AsQueryable().ElementType);
            mockSetUser.As<IQueryable<User>>().Setup(con => con.GetEnumerator()).Returns(testUsers.GetEnumerator());

            var mockContext = new Mock<ApplicationContext>();

            mockContext.Setup(id => id.Users).Returns(mockSetUser.Object);

            IUserService userService = new UserService(mockContext.Object);
            #endregion
            #region act

            var firstUser=(userService.GetPaginatedUsersList(0,1) as IList<User>)[0];
            var secondUser = (userService.GetPaginatedUsersList(1, 1) as IList<User>)[0];
            var thirdUser = (userService.GetPaginatedUsersList(2, 1) as IList<User>)[0];

            #endregion
            #region assert
            Assert.Equal("Vasya", firstUser.Login);
            Assert.Equal("Vitaliy", secondUser.Login);
            Assert.Equal("Nikita", thirdUser.Login);
            #endregion
        }
        [Fact]
        public void GetPaginatedUsersListOfCustomersTest()
        {
            #region arrange
            //Users
            var mockSetUser = new Mock<DbSet<User>>();

            var testUsers = GetTestUsers();

            mockSetUser.As<IQueryable<User>>().Setup(m => m.Provider).Returns(testUsers.AsQueryable().Provider);
            mockSetUser.As<IQueryable<User>>().Setup(m => m.Expression).Returns(testUsers.AsQueryable().Expression);
            mockSetUser.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(testUsers.AsQueryable().ElementType);
            mockSetUser.As<IQueryable<User>>().Setup(con => con.GetEnumerator()).Returns(testUsers.GetEnumerator());
            //Customers
            var mockSetCustomers = new Mock<DbSet<Customer>>();

            var testCustomers = GetFullCustomersList();

            mockSetCustomers.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(testCustomers.AsQueryable().Provider);
            mockSetCustomers.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(testCustomers.AsQueryable().Expression);
            mockSetCustomers.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(testCustomers.AsQueryable().ElementType);
            mockSetCustomers.As<IQueryable<Customer>>().Setup(con => con.GetEnumerator()).Returns(testCustomers.GetEnumerator());

            var mockContext = new Mock<ApplicationContext>();

            mockContext.Setup(id => id.Users).Returns(mockSetUser.Object);
            mockContext.Setup(id => id.Customers).Returns(mockSetCustomers.Object);

            IUserService userService = new UserService(mockContext.Object);
            #endregion
            #region act

            var firstUser = (userService.GetPaginatedUsersListOfCustomers(0, 1) as IList<Customer>)[0];
            var secondUser = (userService.GetPaginatedUsersListOfCustomers(1, 1) as IList<Customer>)[0];
            var thirdUser = (userService.GetPaginatedUsersListOfCustomers(2, 1) as IList<Customer>)[0];

            #endregion
            #region assert

            Assert.Equal("Vitaliy", firstUser.User.Login);
            Assert.Equal("Vasya", secondUser.User.Login);
            Assert.Equal("Nikita", thirdUser.User.Login);
            #endregion

        }
        [Fact]
        public void IsActiveTest()
        {
            #region arrange

            var mockSetUser = new Mock<DbSet<User>>();

            var testUsers = GetTestUsers();

            mockSetUser.As<IQueryable<User>>().Setup(m => m.Provider).Returns(testUsers.AsQueryable().Provider);
            mockSetUser.As<IQueryable<User>>().Setup(m => m.Expression).Returns(testUsers.AsQueryable().Expression);
            mockSetUser.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(testUsers.AsQueryable().ElementType);
            mockSetUser.As<IQueryable<User>>().Setup(con => con.GetEnumerator()).Returns(testUsers.GetEnumerator());

            var mockContext = new Mock<ApplicationContext>();

            mockContext.Setup(id => id.Users).Returns(mockSetUser.Object);

            IUserService userService = new UserService(mockContext.Object);

            #endregion
            #region act
            bool isActiveFirst=userService.IsActive("Vasya");
            #endregion
            #region assert
            Assert.True(isActiveFirst);
            Assert.Throws<UserWasBannedException>(()=> userService.IsActive("Nikita"));
            #endregion

        }
        [Fact]
        public void SuchLoginExistOfTest()
        {
            #region arrange
            var mockSetUser = new Mock<DbSet<User>>();

            var testUsers = GetTestUsers();

            mockSetUser.As<IQueryable<User>>().Setup(m => m.Provider).Returns(testUsers.AsQueryable().Provider);
            mockSetUser.As<IQueryable<User>>().Setup(m => m.Expression).Returns(testUsers.AsQueryable().Expression);
            mockSetUser.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(testUsers.AsQueryable().ElementType);
            mockSetUser.As<IQueryable<User>>().Setup(con => con.GetEnumerator()).Returns(testUsers.GetEnumerator());

            var mockContext = new Mock<ApplicationContext>();

            mockContext.Setup(id => id.Users).Returns(mockSetUser.Object);

            IUserService userService = new UserService(mockContext.Object);

            #endregion
            #region act
            bool existUser=userService.SuchLoginExistOf("Vasya");
            bool unexistUser = userService.SuchLoginExistOf("Oleh");
            #endregion
            #region assert
            Assert.True(existUser);
            Assert.False(unexistUser);
            #endregion
        }
        [Fact]
        public void OperationPossibleTest()
        {
            #region arrange
            //Users
            var mockSetUser = new Mock<DbSet<User>>();

            var testUsers = GetTestUsers();

            mockSetUser.As<IQueryable<User>>().Setup(m => m.Provider).Returns(testUsers.AsQueryable().Provider);
            mockSetUser.As<IQueryable<User>>().Setup(m => m.Expression).Returns(testUsers.AsQueryable().Expression);
            mockSetUser.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(testUsers.AsQueryable().ElementType);
            mockSetUser.As<IQueryable<User>>().Setup(con => con.GetEnumerator()).Returns(testUsers.GetEnumerator());
            //Customers
            var mockSetCustomers = new Mock<DbSet<Customer>>();

            var testCustomers = GetFullCustomersList();

            mockSetCustomers.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(testCustomers.AsQueryable().Provider);
            mockSetCustomers.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(testCustomers.AsQueryable().Expression);
            mockSetCustomers.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(testCustomers.AsQueryable().ElementType);
            mockSetCustomers.As<IQueryable<Customer>>().Setup(con => con.GetEnumerator()).Returns(testCustomers.GetEnumerator());

            var mockContext = new Mock<ApplicationContext>();

            mockContext.Setup(id => id.Users).Returns(mockSetUser.Object);
            mockContext.Setup(id => id.Customers).Returns(mockSetCustomers.Object);

            IUserService userService = new UserService(mockContext.Object);
            #endregion
            #region act
            bool possible1=userService.OperationPossible(1);
            bool possible2=userService.OperationPossible(3);
            bool impossible=userService.OperationPossible(10);
            #endregion
            #region assert
            Assert.True(possible1);
            Assert.True(possible2);
            Assert.False(impossible);
            #endregion
        }
        [Fact]
        public void UpdateUserTest()
        {
            #region arrange
            var mockSetUser = new Mock<DbSet<User>>();

            var testUsers = GetTestUsers();

            mockSetUser.As<IQueryable<User>>().Setup(m => m.Provider).Returns(testUsers.AsQueryable().Provider);
            mockSetUser.As<IQueryable<User>>().Setup(m => m.Expression).Returns(testUsers.AsQueryable().Expression);
            mockSetUser.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(testUsers.AsQueryable().ElementType);
            mockSetUser.As<IQueryable<User>>().Setup(con => con.GetEnumerator()).Returns(testUsers.GetEnumerator());


            var mockContext = new Mock<ApplicationContext>();

            mockContext.Setup(id => id.Users).Returns(mockSetUser.Object);
            //mockSetUser.Setup(m => m.Update(It.IsAny<User>())).Callback<User>((entity) => testUsers.Add(entity));

            IUserService userService = new UserService(mockContext.Object);

            var updatedUser = new User
            {
                Id = 1,
                Phone = "7171111",
                Email = "bask@example.com",
                Login = "Vasya12345",
                PasswordHash = "54321",
                IsActive = true
            };

            #endregion
            #region act

            var updatedUserId=userService.UpdateUser(1, updatedUser);
            var findedUpdatedUser = mockContext.Object.Users.SingleOrDefault(us=>us.Id.Equals(updatedUserId));
            #endregion
            #region assert
            Assert.Equal(updatedUser.Id, findedUpdatedUser.Id);
            Assert.Equal(updatedUser.Login, findedUpdatedUser.Login);
            Assert.Equal(updatedUser.IsActive, findedUpdatedUser.IsActive);
            Assert.Equal(updatedUser.PasswordHash, findedUpdatedUser.PasswordHash);
            Assert.Equal(updatedUser.Phone, findedUpdatedUser.Phone);
            Assert.Equal(updatedUser.Email, findedUpdatedUser.Email);
            #endregion
        }
        [Fact]
        public void BlockUnblockUserTest()
        {
            #region arrange
            var mockSetUser = new Mock<DbSet<User>>();

            var testUsers = GetTestUsers();

            mockSetUser.As<IQueryable<User>>().Setup(m => m.Provider).Returns(testUsers.AsQueryable().Provider);
            mockSetUser.As<IQueryable<User>>().Setup(m => m.Expression).Returns(testUsers.AsQueryable().Expression);
            mockSetUser.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(testUsers.AsQueryable().ElementType);
            mockSetUser.As<IQueryable<User>>().Setup(con => con.GetEnumerator()).Returns(testUsers.GetEnumerator());

            var mockContext = new Mock<ApplicationContext>();

            mockContext.Setup(id => id.Users).Returns(mockSetUser.Object);

            IUserService userService = new UserService(mockContext.Object);

            #endregion
            #region act

            var updatedActiveStatusUserId1 = userService.BlockUnblockUser(1);
            var updatedActiveStatusUserId2 = userService.BlockUnblockUser(3);
            mockSetUser.Verify(m => m.Update(It.IsAny<User>()), Times.Exactly(2));
            mockContext.Verify(m => m.SaveChanges(), Times.Exactly(2));
            #endregion
            #region assert
            Assert.False(mockContext.Object.Users.SingleOrDefault(us=>us.Id.Equals(updatedActiveStatusUserId1)).IsActive);
            Assert.True(mockContext.Object.Users.SingleOrDefault(us => us.Id.Equals(updatedActiveStatusUserId2)).IsActive);
            #endregion
        }
        #region FillDataMethods
        private IList<User> GetTestUsers()
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
        private IList<Admin> GetTestAdmins()
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
        private IList<Customer> GetTestCustomers()
        {
            var customers = new List<Customer>()
            {
                new Customer()
                {
                    Id=1,
                    UserId=2,
                }

            };
            return customers;
        }
        private IList<Worker> GetTestWorkers()
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
        private IList<Customer> GetFullCustomersList()
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
        #endregion
    }
}
