using GladosBank.Domain.Models;
using System.Collections.Generic;

namespace GladosBank.Services
{
    public interface ICurrencyService
    {
        IEnumerable<Currency> GetAllCurrenciesService();
        string GetCurrencyFromId(int id);
    }
}