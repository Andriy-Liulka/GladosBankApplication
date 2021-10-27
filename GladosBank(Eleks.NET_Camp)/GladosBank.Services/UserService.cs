using GladosBank.Domain;
using GladosBank.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Services
{
    public class UserService
    {
        public UserService(ApplicationContext context)
        {
            _context = context;
        }
        #region Create
        public int CreateUser(User user)
        {
            if (CheckWhetherSuchUserExist(user))
            {
                throw new AddingExistUserException("You try to add user that already exist of !!");
            }
            _context.Users.Add(user);
            _context.SaveChanges();
            


            return user.Id;
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
                return null;
            }
            return searchedUser;
        }
        
        public User[] GetAllUsers()
        {
            //ToDO paginning
            var users = _context.Users.ToArray();
            //users.ToList<User>().Take<User>(10);
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

        private readonly ApplicationContext _context;


    }
}
