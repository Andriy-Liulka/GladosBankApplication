using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Api.Config.Athentication
{
    public class JwtAuthenticationOptions
    {
        public const string Issuer = "GladosBank"; 
        public const string Audience = "GladosBank"; 
        internal const string Key = "securityTokenGladosBank1488"; 
        public const int LifeTime = 15;
        public static RsaSecurityKey GetSymmetricSecurityKey()
        {
            
            RSA rsa = RSA.Create();
            byte[] rawKey = Encoding.ASCII.GetBytes(Key);
            //rsa.ImportRSAPrivateKey(rawKey, out _);
            var privateKey = new RsaSecurityKey(rsa);
            return privateKey;
        }
    }
}
