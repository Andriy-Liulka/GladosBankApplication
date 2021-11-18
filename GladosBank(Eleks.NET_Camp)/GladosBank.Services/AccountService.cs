
using GladosBank.Domain;
using GladosBank.Domain.Models;
using GladosBank.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
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

            #region AnotherPossibleSolution
            //var account =
            //                from user in _context.Users
            //                join Customers in _context.Customers on user.Id equals Customers.UserId
            //                select new { CustomerId = Customers.Id, Login = user.Login };
            #endregion
            var account = _context.Customers.Include(us => us.User).SingleOrDefault(cs=>cs.User.Login.Equals(login));

            return account.Id;
        }
        public IEnumerable<Account> GetAllAccounts(string login)
        {
            int? currentCustomerId = GetCustomerIdFromLogin(login);
            if (currentCustomerId == null)
            {
                throw new IsntCustomerException(login);
            }

            var accounts = _context.Accounts.Include(acc => acc.Currency).Where(acc => acc.CustomerId.Equals(currentCustomerId)).ToArray();

            #region AnotherPossibleSolution
            //One of possible solutions
            //var accounts =
            //    (from account in _context.Accounts
            //    join currency in _context.Currency
            //    on account.CurrencyCode equals currency.Code where account
            //     .CustomerId.Equals(currentCustomerId) select new
            //    {
            //        AccountId=account.Id,
            //        CustomerId = account.CustomerId,
            //        CurrencyCode = $"{currency.Symbol} ({currency.Code})",
            //        Amount = account.Amount,
            //        Notes = account.Notes,
            //        DateOfCreating=account.DateOfCreating
            //    }).ToArray();
            #endregion

            return accounts;
        }
        public IEnumerable<Currency> GetAllCurrencies()
        {
            return _context.Currency;
        }

        #endregion
        #region Update
        public int ReplenishAccount(int Id,decimal amount)
        {
            var account=_context.Accounts.FirstOrDefault(acc=>acc.Id.Equals(Id));
            if (account == null)
            {
                throw new InvalidAccountIdExcepion(Id);
            }
            account.Amount += amount;
            _context.Accounts.Update(account);
            _context.SaveChanges();
            return Id;
        }
        #endregion
        #region Delete
        public int DeleteAccount(int accountId)
        {
           var account= _context.Accounts.FirstOrDefault(acc=>acc.Id.Equals(accountId));
            if (account!=null)
            {
                _context.Accounts.Remove(account);
                _context.SaveChanges();
                return account.Id;
            }
            else
            {
                throw new InvalidAccountIdExcepion(accountId);
            }
        }
        #endregion
        private readonly ApplicationContext _context;
    }
}
