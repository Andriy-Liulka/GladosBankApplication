using GladosBank.Domain;
using System.Collections.Generic;

namespace GladosBank.Services
{
    public interface IOperationHistoryService
    {
        IEnumerable<OperationsHistory> GetTransactionHistoryElementService(int pageIndex, int pageSize, int customerId);
        int KeepHistoryElementOfOperation(OperationsHistory operation);
    }
}