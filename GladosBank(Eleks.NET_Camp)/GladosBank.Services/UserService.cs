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
            SetDefaultIsActiveToUser(user);

            _context.Users.Add(user);
            _context.SaveChanges();
            SetRoleToSpecifiedUser(user,role);
            _context.SaveChanges();
            return user.Id;
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
                        _context.Workers.Add(new Worker { UserId = user.Id,Salary=default });
                        break;
                    }
                default:
                    {
                        throw  new InvalidRoleException("GladosBank hasn't such role");
                    }
            }
        }
        private void SetDefaultIsActiveToUser(User user)
        {
            user.IsActive = true;
        }
        public bool CheckWhetherSuchUserExist(User user)
        {
            User checkUser = _context.Users.FirstOrDefault<User>(us => us.Id == user.Id);
            return user.Equals(checkUser);
        }

        #endregion
        #region Get
        public User GetUser(int UserId)
        {
            User searchedUser = _context.Users.FirstOrDefault<User>(user=>user.Id== UserId);
            if (searchedUser == null)
            {
                throw new InvalidUserIdException("You entered UserId that doesn't exist of in database !");
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
                throw new InvalidUserIdException("You entered UserId that doesn't exist of in database !");
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
                throw new InvalidUserIdException("You entered UserId that doesn't exist of in database !");
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
            distination.Password = source.Password;
            distination.Phone = source.Phone;
        }
        #endregion
        private readonly ApplicationContext _context;
    }
}
