using GladosBank.Domain;
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
        public int CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.Id;
        }
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

        public int DeleteUser(int userId)
        {
            User existingUser = _context.Users.FirstOrDefault<User>(u=>u.Id==userId);
            if (existingUser==null)
            {
                return -1;
            }
            _context.Users.Remove(existingUser);
            _context.SaveChanges();
            return userId;
        }

        
        private readonly ApplicationContext _context;


    }
}
