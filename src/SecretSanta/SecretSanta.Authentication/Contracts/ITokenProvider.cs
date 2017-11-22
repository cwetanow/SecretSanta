using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace SecretSanta.Authentication.Contracts
{
    public interface ITokenProvider
    {
        string GenerateToken(string issuer, string audience, IEnumerable<Claim> claims, DateTime expires, SigningCredentials signingCredentials);
    }
}
