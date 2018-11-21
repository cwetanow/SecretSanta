using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace SecretSanta.Providers.Contracts
{
    public interface ITokenProvider
    {
        string GenerateToken(string issuer, 
            string audience, 
            IEnumerable<Claim> claims, 
            DateTime expires, 
            SigningCredentials signingCredentials);
    }
}
