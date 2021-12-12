using GladosBank.Domain;
using GladosBank.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Services
{
    public class OperationHistoryService : IOperationHistoryService
    {
        public OperationHistoryService(ApplicationContext context)
        {
            _context = context;
        }
        public int KeepHistoryElementOfOperation(OperationsHistory operation)
        {
            if (!CustomerExist(operation.CustomerId))
            {
                throw new InvalidCustomerException(operation.CustomerId);
            }
            _context.OperationsHistory.Add(operation);
            _context.SaveChanges();
            var operationId = _context.OperationsHistory
                .FirstOrDefault(cus => cus.CustomerId
                .Equals(operation.CustomerId)).Id;

            return operationId;
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
        public bool CustomerExist(int CustomerId)
        {
            return _context.Customers.Any(cus => cus.Id.Equals(CustomerId));

        }

        private readonly ApplicationContext _context;
    }
}
