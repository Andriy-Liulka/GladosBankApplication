using AutoMapper;
using GladosBank.Domain;
using GladosBank.Services.Exceptions;
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
            if (CheckWhetherSuchUserExist(user))
            {
                throw new AddingExistUserException("You try to add user that already exist of !!");
            }
            else if(IsSuchLoginInDatabase(user.Login))
            {
                throw new ExistingUserLoginException("Such login already exist of !");
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
                        _context.Workers.Add(new Worker { UserId = user.Id });
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
        
        public IEnumerable<User> GetAllUsers()
        {
            //ToDO paginning
            var users = _context.Users.ToArray();
            return users;
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
            var existingUser = _context.Users.FirstOrDefault(us => us.Id == UserId);
            if (existingUser==null)
            {
                throw new InvalidUserIdException(UserId);
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
        #endregion
        private readonly ApplicationContext _context;
    }
}
