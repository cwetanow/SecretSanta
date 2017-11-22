using Microsoft.IdentityModel.Tokens;
using SecretSanta.Authentication.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace SecretSanta.Authentication
{
    public class JwtTokenProvider : ITokenProvider
    {
        public string GenerateToken(string issuer, string audience, IEnumerable<Claim> claims, DateTime expires, SigningCredentials signingCredentials)
        {
            var token = new JwtSecurityToken(issuer: issuer,
                       audience: audience,
                       claims: claims,
                       expires: expires,
                       signingCredentials: signingCredentials);

            var handler = new JwtSecurityTokenHandler();

            return handler.WriteToken(token);
        }
    }
}
