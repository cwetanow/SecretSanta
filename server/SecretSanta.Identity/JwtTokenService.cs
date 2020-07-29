using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SecretSanta.Identity.Configuration;
using SecretSanta.Identity.Interfaces;

namespace SecretSanta.Identity
{
	public class JwtTokenService : ITokenService
	{
		private const string UserDisplayNameClaim = "UserDisplayName";

		private readonly JwtAuthConfiguration configuration;

		public JwtTokenService(IOptions<JwtAuthConfiguration> configurationOptions)
		{
			this.configuration = configurationOptions.Value;
		}

		public string EncodeToken(string username, int userId, string displayName)
		{
			var claims = new List<Claim> {
				new Claim(JwtRegisteredClaimNames.Sub, username),
				new Claim(JwtRegisteredClaimNames.Jti, userId.ToString()),
				new Claim(UserDisplayNameClaim,displayName)
			};

			var expires = DateTime.UtcNow.AddHours(configuration.ValidHours);

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.SecretKey));

			var tokenDescriptor = new SecurityTokenDescriptor {
				Subject = new ClaimsIdentity(claims),
				Expires = expires,
				SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
				Issuer = configuration.Issuer,
				Audience = configuration.Audience
			};

			var handler = new JwtSecurityTokenHandler();

			var token = handler.CreateToken(tokenDescriptor);

			return handler.WriteToken(token);
		}
	}
}
