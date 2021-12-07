using GladosBank.Domain;
using GladosBank.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GladosBank.Services
{
    public interface IAccountService
    {
        int CreateAccount(Account account);
        int DeleteAccount(int accountId);
        IEnumerable<Account> GetAllAccountsForCurrencyCode(string currencyCode, string login);
        IEnumerable<Currency> GetAllCurrenciesService();
        Task<IEnumerable<Account>> GetAllUserAccounts(string login);
        string GetCurrencyFromId(int id);
        int GetCustomerIdFromLogin(string login);
        Task<IEnumerable<OperationsHistory>> GetTransactionHistoryElementService(int pageIndex, int pageSize, int customerId);
        int ReplenishAccount(int Id, decimal amount);
        (int, int) TransferMoney(decimal amount, int sourceId, int destinationId);
    }
}