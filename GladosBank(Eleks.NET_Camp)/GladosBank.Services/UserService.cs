using GladosBank.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Services
{
    class UserService
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



        private readonly ApplicationContext _context;


    }
}
