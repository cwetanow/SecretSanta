using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using SecretSanta.Providers.Contracts;

namespace SecretSanta.Providers
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
