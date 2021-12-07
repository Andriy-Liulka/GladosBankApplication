using System.Collections.Generic;
using System.Security.Claims;

namespace GladosBank.Services
{
    public interface IDataService
    {
        int GetCustomerId(IEnumerable<Claim> claims);
        string GetEmail(IEnumerable<Claim> claims);
        string GetLogin(IEnumerable<Claim> claims);
        string GetPhone(IEnumerable<Claim> claims);
        string GetRole(IEnumerable<Claim> claims);
        bool IsYourAccount(IEnumerable<Claim> claims, int accountId);
    }
}