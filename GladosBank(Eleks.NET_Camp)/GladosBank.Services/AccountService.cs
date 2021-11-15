
using GladosBank.Domain;
using GladosBank.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Services
{
    public sealed class AccountService
    {
        public AccountService(ApplicationContext context)
        {
            _context = context;
        }
        #region Create
        public int CreateAccount(Account account)
        {

            bool accountExist=_context.Accounts.Any(us=>us.Id== account.Id);
            if (accountExist == true) 
            { 
                throw new ExistingAccountException(account.Id);
            }

            bool customerExist = _context.Customers.Any(us => us.Id == account.CustomerId);
            if (customerExist == false)
            {
                throw new InvalidCustomerException(account.CustomerId);
            }

            bool currencyCodeExist = _context.Accounts.Any(us => us.CurrencyCode.Equals(account.CurrencyCode));
            if (customerExist == false)
            {
                throw new InvalidCurrencyException(account.CurrencyCode);
            }

            _context.Accounts.Add(account);
            _context.SaveChanges();
            return account.Id;
        }

        #endregion
        #region Get
        
        public IEnumerable<Account> GetAllAccounts(string login)
        {
            //var accounts = _context.Users.Join(_context.Customers,us=>us.Id,us2=>us2.UserId,).ToArray();

            var account =
                from user in _context.Users
                join Customers in _context.Customers on user.Id equals Customers.UserId
                select new { CustomerId = Customers.Id,Login=user.Login };

            var currentCustomerId = account.FirstOrDefault(ac => ac.Login.Equals(login));
            if (currentCustomerId == null)
            {
                throw new IsntCustomerException(login);
            }
            var accounts=_context.Accounts.Where(ci => ci.CustomerId == currentCustomerId.CustomerId);

            return accounts;
        }
        #endregion
        #region Update

        #endregion
        #region Delete

        #endregion
        private readonly ApplicationContext _context;
    }
}
