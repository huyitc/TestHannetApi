﻿using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hannet.Service
{
    public interface IIdentityService
    {
        string GenerateJwtToken(string userId, string userName, List<string> roles, string secret);
    }
    public class IdentityService : IIdentityService
    {
        public string GenerateJwtToken(string userId, string userName, List<string> roles, string secret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            List<Claim> claims = new List<Claim>();
            foreach (var role in roles)
            {
                var roleClaim = new Claim(ClaimTypes.Role, role);
                claims.Add(roleClaim);
            }
            var claims1 = new List<Claim>(claims) {
                new Claim(userId, userName)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //Subject = new ClaimsIdentity(new[]
                //{
                //    new Claim(ClaimTypes.NameIdentifier, userId),
                //    new Claim(ClaimTypes.Name, userName),

                //}),
                Subject = new ClaimsIdentity(claims1),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encryptedToken = tokenHandler.WriteToken(token);

            return encryptedToken;
        }
    }
}
