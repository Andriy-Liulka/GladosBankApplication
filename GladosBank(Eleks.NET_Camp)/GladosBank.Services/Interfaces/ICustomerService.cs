using GladosBank.Domain;
using System.Collections.Generic;

namespace GladosBank.Services
{
    public interface ICustomerService
    {
        bool CustomerExist(int CustomerId);
        int GetCustomerIdFromLogin(string login);
        IEnumerable<Customer> GetPaginatedUsersListOfCustomers(int pageIndex, int pageSize);
    }
}