﻿using GladosBank.Domain;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GladosBank.Api.Config.Athentication
{
    public class JwtGenerator
    {
        public string CreateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenBody = new SecurityTokenDescriptor
            {
                Audience = JwtAuthenticationOptions.Audience,
                Issuer = JwtAuthenticationOptions.Issuer,
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Login),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.MobilePhone, user.Phone)
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials=new SigningCredentials(JwtAuthenticationOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.RsaSha256)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenBody);
            string jwtToken = tokenHandler.WriteToken(token);
            return jwtToken;
        }




    }
}
