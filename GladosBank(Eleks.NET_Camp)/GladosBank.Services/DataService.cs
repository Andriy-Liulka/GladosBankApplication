using GladosBank.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Services
{
    public sealed class DataService
    {
        public DataService(ApplicationContext context)
        {
            _context = context;
        }

        public string GetLogin(IEnumerable<Claim> claims)
        {
            Claim claim = claims.FirstOrDefault(us => us.Type.Equals(ClaimTypes.Name));
            return claim?.Value;
        }
        public int GetCustomerId(IEnumerable<Claim> claims)
        {
            string login = claims.FirstOrDefault(us => us.Type.Equals(ClaimTypes.Name))?.Value;
            int customerId = _context.Customers
                .Include(cus => cus.User)
                .SingleOrDefault(us=>us.User.Login.Equals(login))
                .Id;
            return customerId;
        }

        public string GetEmail(IEnumerable<Claim> claims)
        {
            Claim claim = claims.FirstOrDefault(us => us.Type.Equals(ClaimTypes.Email));
            return claim?.Value;
        }

        public string GetRole(IEnumerable<Claim> claims)
        {
            Claim claim = claims.FirstOrDefault(us => us.Type.Equals(ClaimTypes.Role));
            return claim?.Value;
        }

        public string GetPhone(IEnumerable<Claim> claims)
        {
            Claim claim = claims.FirstOrDefault(us => us.Type.Equals(ClaimTypes.MobilePhone));
            return claim?.Value;
        }

        public bool IsYourAccount(IEnumerable<Claim> claims,int accountId)
        {
            Claim claim = claims.FirstOrDefault(us => us.Type.Equals(ClaimTypes.Name));
            string Login= claim?.Value;

            return _context.Accounts
                .Include(cus => cus.Customer)
                .ThenInclude(us => us.User)
                .Where(acc=>acc.Id.Equals(accountId))
                .Any(us => us.Customer.User.Login.Equals(Login));

            
        }

        private readonly ApplicationContext _context;
    }
}
