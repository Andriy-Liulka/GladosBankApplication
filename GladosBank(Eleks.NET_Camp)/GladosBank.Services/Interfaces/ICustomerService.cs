using GladosBank.Domain;
using System.Collections.Generic;

namespace GladosBank.Services
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetPaginatedUsersListOfCustomers(int pageIndex, int pageSize);
    }
}