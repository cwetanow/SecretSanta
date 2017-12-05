using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SecretSanta.Authentication.Contracts;
using SecretSanta.Common;
using SecretSanta.Providers.Contracts;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SecretSanta.Authentication
{
    public class TokenManager : ITokenManager
    {
        private readonly IConfiguration configuration;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly ITokenProvider tokenProvider;

        public TokenManager(IConfiguration configuration, IDateTimeProvider dateTimeProvider, ITokenProvider tokenProvider)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (dateTimeProvider == null)
            {
                throw new ArgumentNullException(nameof(dateTimeProvider));
            }

            if (tokenProvider == null)
            {
                throw new ArgumentNullException(nameof(tokenProvider));
            }

            this.configuration = configuration;
            this.dateTimeProvider = dateTimeProvider;
            this.tokenProvider = tokenProvider;
        }

        public string GenerateToken(string email)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var tokenKey = this.configuration[Constants.TokenKey];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var issuer = this.configuration[Constants.TokenIssuer];

            var expireDate = this.dateTimeProvider.GetTimeFromCurrentTime(Constants.TokenExpireHours,
                Constants.TokenExpireMinutes, Constants.TokenExpireSeconds);

            var token = this.tokenProvider.GenerateToken(issuer, issuer, claims, expireDate, credentials);

            return token;
        }
    }
}
