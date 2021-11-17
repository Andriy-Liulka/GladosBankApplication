
using GladosBank.Domain;
using GladosBank.Domain.Models;
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
        public int GetCustomerIdFromLogin(string login)
        {
            var account =
                from user in _context.Users
                join Customers in _context.Customers on user.Id equals Customers.UserId
                select new { CustomerId = Customers.Id, Login = user.Login };

            var currentCustomerId = account.FirstOrDefault(ac => ac.Login.Equals(login));
            return currentCustomerId.CustomerId;
        }
        public IEnumerable<object> GetAllAccounts(string login)
        {
            int? currentCustomerId = GetCustomerIdFromLogin(login);
            if (currentCustomerId == null)
            {
                throw new IsntCustomerException(login);
            }
            var accounts =
                (from account in _context.Accounts
                join currency in _context.Currency
                on account.CurrencyCode equals currency.Code select new
                {
                    CustomerId = account.CustomerId,
                    CurrencyCode = $"{currency.Symbol} ({currency.Code})",
                    Amount = account.Amount,
                    Notes = account.Notes,
                    DateOfCreating=account.DateOfCreating
                }).ToArray();


            return accounts;
        }
        public IEnumerable<Currency> GetAllCurrencies()
        {
            return _context.Currency;
        }

        #endregion
        #region Update

        #endregion
        #region Delete

        #endregion
        private readonly ApplicationContext _context;
    }
}
