using GladosBank.Domain;
using System.Collections.Generic;

namespace GladosBank.Services
{
    public interface IAccountService
    {
        int CreateAccount(Account account);
        int DeleteAccount(int accountId);
        Account GetAccountFromId(int accountId);
        IEnumerable<Account> GetAllAccountsForCurrencyCode(string currencyCode, string login);
        IEnumerable<Account> GetAllUserAccounts(string login);
        int ReplenishAccount(int Id, decimal amount);
        bool TransferMoney(decimal amount, Account source, Account destination);
        bool TransferMoneySaver(decimal amount, Account source, Account destination);
    }
}