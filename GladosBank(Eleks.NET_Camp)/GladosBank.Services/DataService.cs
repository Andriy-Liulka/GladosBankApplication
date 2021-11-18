using GladosBank.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Services
{
    public sealed class DataService
    {
        public DataService(ApplicationContext context)
        {
            _context = context;
        }

        public string GetLogin(IEnumerable<Claim> claims)
        {
            Claim claim = claims.FirstOrDefault(us => us.Type.Equals(ClaimTypes.Name));
            return claim?.Value;
        }

        public string GetEmail(IEnumerable<Claim> claims)
        {
            Claim claim = claims.FirstOrDefault(us => us.Type.Equals(ClaimTypes.Email));
            return claim?.Value;
        }

        public string GetRole(IEnumerable<Claim> claims)
        {
            Claim claim = claims.FirstOrDefault(us => us.Type.Equals(ClaimTypes.Role));
            return claim?.Value;
        }

        private readonly ApplicationContext _context;
    }
}
