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
        public OperationHistoryService(ApplicationContext context, CustomerService custService)
        {
            _context = context;
            _custService = custService;
        }
        public int KeepHistoryElementOfOperation(OperationsHistory operation)
        {
            if (!_custService.CustomerExist(operation.CustomerId))
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

        private readonly ApplicationContext _context;
        private readonly CustomerService _custService;
    }
}
