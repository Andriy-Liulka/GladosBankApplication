using GladosBank.Domain;
using GladosBank.Domain.Models;
using System.Collections.Generic;

namespace GladosBank.Services
{
    public interface IAccountService
    {
        int CreateAccount(Account account);
        int DeleteAccount(int accountId);
        Account GetAccountFromId(int accountId);
        IEnumerable<Account> GetAllAccountsForCurrencyCode(string currencyCode, string login);
        IEnumerable<Currency> GetAllCurrenciesService();
        IEnumerable<Account> GetAllUserAccounts(string login);
        string GetCurrencyFromId(int id);
        int GetCustomerIdFromLogin(string login);
        IEnumerable<OperationsHistory> GetTransactionHistoryElementService(int pageIndex, int pageSize, int customerId);
        int ReplenishAccount(int Id, decimal amount);
        bool TransferMoney(decimal amount, Account source, Account destination);
        bool TransferMoneySaver(decimal amount, Account source, Account destination);
    }
}