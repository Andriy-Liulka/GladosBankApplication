using AutoMapper;
using GladosBank.Domain;
using GladosBank.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Services
{
    public sealed class UserService
    {
        public UserService(ApplicationContext context)
        {
            _context = context;
        }
        #region Create  
        public int CreateUser(User user,string role)
        {
            //Just to work,becouse FluentAPI has buggs.
            user.IsActive = true;

            if (CheckWhetherSuchUserExist(user))
            {
                throw new AddingExistUserException();
            }
            else if(IsSuchLoginInDatabase(user.Login))
            {
                throw new ExistingUserLoginException(user.Login);
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Users.Add(user);
                    _context.SaveChanges();
                    SetRoleToSpecifiedUser(user, role);
                    _context.SaveChanges();
                    transaction.Commit();
                    return user.Id;
                }
                catch (InvalidRoleException ex)
                {
                    throw new InvalidRoleException(ex.Message);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return (0);
                }
            }


        }


        private bool IsSuchLoginInDatabase(string login)
        {
            return _context.Users.Any(us=>us.Login.Equals(login));
        }
        private void SetRoleToSpecifiedUser(User user, string role)
        {
            switch (role)
            {
                case "Customer":
                    {
                        _context.Customers.Add(new Customer {UserId= user.Id});
                        break;
                    }
                case "Admin":
                    {
                        _context.Admins.Add(new Admin { UserId = user.Id });
                        break;
                    }
                case "Worker":
                    {
                        //Just to work,becouse FluentAPI has buggs.
                        _context.Workers.Add(new Worker { UserId = user.Id,Salary=0 });
                        break;
                    }
                default:
                    {
                        throw  new InvalidRoleException(role);
                    }
            }

        }

        public bool CheckWhetherSuchUserExist(User user)
        {
            return _context.Users.Any<User>(us => us.Id == user.Id);
        }

        #endregion
        #region Get
        public User GetUserByLogin(string Login)
        {
            User searchedUser = _context.Users.FirstOrDefault<User>(user=>user.Login== Login);
            if (searchedUser == null)
            {
                throw new InvalidUserLoginException(Login);
            }
            return searchedUser;
        }
        public string GetRole(string login)
        {
            var existingUserId = _context.Users.FirstOrDefault(us=>us.Login == login).Id;
            if(_context.Customers.Any(us=>us.UserId == existingUserId))
            {
                return "Customer";
            }
            else if (_context.Workers.Any(us => us.UserId == existingUserId))
            {
                return "Worker";
            }
            else if (_context.Admins.Any(us => us.UserId == existingUserId))
            {
                return "Admin";
            }
            throw new DonotHaveRoleException(login);
        }
        public IEnumerable<User> GetAllUsers()
        {
            //ToDO paginning
            var users = _context.Users.ToArray();
            return users;
        }

        public async Task<IEnumerable<User>> GetPaginatedUsersList(int pageIndex,int pageSize)
        {
            int generalSkipSize = pageIndex * pageSize;
            var users = await _context.Users
                .Take((generalSkipSize) + pageSize)
                .Skip(generalSkipSize)
                .ToArrayAsync();
            return users;
        }
        public async Task<IEnumerable<Customer>> GetPaginatedUsersListOfCustomers(int pageIndex, int pageSize)
        {
            int generalSkipSize = pageIndex * pageSize;
            var customers=await _context.Customers
                .Include(us=>us.User)
                .Take((generalSkipSize) + pageSize)
                .Skip(generalSkipSize)
                .ToArrayAsync();

            return customers;
        }
        
        #endregion
        #region Delete
        public int DeleteUser(int userId)
        {
            User existingUser = _context.Users.FirstOrDefault<User>(u=>u.Id==userId);
            if (existingUser==null)
            {
                throw new InvalidUserIdException(userId);
            }
            _context.Users.Remove(existingUser);
            _context.SaveChanges();
            return userId;
        }
        #endregion
        #region Update
        public int UpdateUser(int UserId, User user)
        {
            var existingUser = _context.Users.SingleOrDefault(us => us.Id == UserId);
            if (existingUser==null)
            {
                throw new InvalidUserIdException(UserId);
            }
            if (SuchLoginExistOf(user.Login))
            {
                throw new ExistingUserLoginException(user.Login);
            }
            UpdateFields(user, existingUser);
            _context.Update(existingUser);
            _context.SaveChanges();

            return UserId;
        }

        private static void UpdateFields(User source,User distination)
        {
            distination.Email = source.Email;
            distination.IsActive = source.IsActive;
            distination.Login = source.Login;
            distination.PasswordHash = source.PasswordHash;
            distination.Phone = source.Phone;
        }
        public int BlockUnblockUser(int userId)
        {
            User existingUser = _context.Users.SingleOrDefault(us=>us.Id.Equals(userId));
            if (existingUser == null)
            {
                throw new InvalidAccountIdExcepion(userId);
            }
            existingUser.IsActive = !existingUser.IsActive;
            _context.Users.Update(existingUser);
            _context.SaveChanges();
            return existingUser.Id;
        }
        #endregion
        #region CheckNess
        public bool IsActive(string login)
        {
           var existingUser= _context.Users.SingleOrDefault(us => us.Login.Equals(login));

           if (existingUser == null)
           {
               throw new InvalidUserLoginException(login);
           }

            if (!existingUser.IsActive)
            {
                throw new UserWasBannedException(login);
            }
            return true;
        }

        public bool SuchLoginExistOf(string login)
        {
            return _context.Users.Any(us=>us.Login.Equals(login));
        }
        #endregion

        private readonly ApplicationContext _context;
    }
}
