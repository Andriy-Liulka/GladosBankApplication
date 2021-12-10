
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
    public sealed class AccountService : IAccountService
    {
        public AccountService(ApplicationContext context, UserService userService)
        {
            _userService = userService;
            _context = context;
        }
        #region Create
        public int CreateAccount(Account account)
        {

            bool accountExist = _context.Accounts.Any(us => us.Id == account.Id);
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
            var account = _context.Customers
                .Include(us => us.User)
                .FirstOrDefault(cs => cs.User.Login.Equals(login));
            //SingleOrDefault

            return account.Id;
        }
        public IEnumerable<Account> GetAllUserAccounts(string login)
        {
            int? currentCustomerId = GetCustomerIdFromLogin(login);
            if (currentCustomerId == null)
            {
                throw new IsntCustomerException(login);
            }

            var accounts = _context.Accounts
            .Include(acc => acc.Currency)
            .Where(acc => acc.CustomerId.Equals(currentCustomerId))
            .ToList();

            return accounts;



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


        }
        public IEnumerable<Currency> GetAllCurrenciesService()
        {
            return _context.Currency.ToArray();
        }
        public string GetCurrencyFromId(int id)
        {
            var currency = _context.Accounts.SingleOrDefault(us => us.Id.Equals(id)).CurrencyCode;
            if (currency == null)
            {
                throw new InvalidAccountIdExcepion(id);
            }
            return currency;
        }
        public IEnumerable<Account> GetAllAccountsForCurrencyCode(string currencyCode, string login)
        {
            var userId = _userService.GetUserByLogin(login).Id;

            var accounts = _context.Accounts
                .Include(cus => cus.Customer)
                .ThenInclude(us => us.User)
                .Where(ac => ac.CurrencyCode.Equals(currencyCode)).Where(cus => cus.Customer.UserId.Equals(userId)).ToArray();

            return accounts;
        }
        public IEnumerable<OperationsHistory> GetTransactionHistoryElementService(int pageIndex, int pageSize, int customerId)
        {
            int generalSkipSize = pageIndex * pageSize;
            var historyElements = _context.OperationsHistory
                .Where(op => op.CustomerId.Equals(customerId))
                .Take((generalSkipSize) + pageSize)
                .Skip(generalSkipSize)
                .ToArray();
            return historyElements;
        }
        public Account GetAccountFromId(int accountId)
        {
            return _context.Accounts.SingleOrDefault(acc => acc.Id.Equals(accountId));
        }
        #endregion
        #region Update

        public int ReplenishAccount(int Id, decimal amount)
        {
            var account = _context.Accounts.FirstOrDefault(acc => acc.Id.Equals(Id));
            if (account == null)
            {
                throw new InvalidAccountIdExcepion(Id);
            }
            if (amount <= 0)
            {
                throw new SmallAmountException(amount.ToString());
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
            var account = _context.Accounts.FirstOrDefault(acc => acc.Id.Equals(accountId));
            if (account != null)
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
        #region Transaction
        public bool TransferMoneySaver(decimal amount, Account source, Account destination)
        {
            bool result=TransferMoney(amount, source, destination);
            _context.Update(source);
            _context.Update(destination);

            _context.SaveChanges();
            return result;
        }
        public bool TransferMoney(decimal amount, Account source, Account destination)
        {
            source = source ?? throw new ArgumentNullException(nameof(source), $"Account with id {source.Id} doesn't exist of");
            destination = destination ?? throw new ArgumentNullException(nameof(destination), $"Account with id {destination.Id} doesn't exist of");

            if (amount<=0)
            {
                throw new ArgumentOutOfRangeException("Amount too little !");
            }
            if (!source.CurrencyCode.Equals(destination.CurrencyCode))
            {
                throw new DifferentCurrencyException(source.CurrencyCode, destination.CurrencyCode);
            }
            if (source.Amount < amount)
            {
                throw new TooLittleAccountAmountException();
            }
            if (source.Id == destination.Id)
            {
                throw new TheSameAccountException();
            }

            try
            {
                source.Amount -= amount;
                destination.Amount += amount;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
        private readonly ApplicationContext _context;
        private readonly UserService _userService;
    }
}
