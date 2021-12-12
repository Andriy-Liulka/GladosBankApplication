using GladosBank.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Services
{
    public class CustomerService : ICustomerService
    {
        public CustomerService(ApplicationContext context)
        {
            _context = context;
        }
        public IEnumerable<Customer> GetPaginatedUsersListOfCustomers(int pageIndex, int pageSize)
        {
            int generalSkipSize = pageIndex * pageSize;
            var customers = _context.Customers
                .Include(us => us.User)
                .Take((generalSkipSize) + pageSize)
                .Skip(generalSkipSize)
                .ToArray();

            return customers;
        }
        public int GetCustomerIdFromLogin(string login)
        {

            var account = _context.Customers
                .Include(us => us.User)
                .FirstOrDefault(cs => cs.User.Login.Equals(login));

            return account.Id;
        }
        public bool CustomerExist(int CustomerId)
        {
            return _context.Customers.Any(cus => cus.Id.Equals(CustomerId));

        }

        private readonly ApplicationContext _context;
    }
}
