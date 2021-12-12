using GladosBank.Domain;
using GladosBank.Domain.Models;
using GladosBank.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Services
{
    public class CurrencyService : ICurrencyService
    {
        public CurrencyService(ApplicationContext context)
        {
            _context = context;
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

        private readonly ApplicationContext _context;
    }
}
