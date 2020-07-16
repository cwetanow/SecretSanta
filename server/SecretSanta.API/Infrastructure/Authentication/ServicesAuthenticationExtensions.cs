using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SecretSanta.Identity.Configuration;

namespace SecretSanta.API.Infrastructure.Authentication
{
	public static class ServicesAuthenticationExtensions
	{
		public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services, JwtAuthConfiguration jwtConfiguration)
		{
			services
				.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
					options => {
						options.TokenValidationParameters = new TokenValidationParameters {
							IssuerSigningKey =
								new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.SecretKey)),
							ValidAudience = jwtConfiguration.Audience,
							ValidIssuer = jwtConfiguration.Issuer,
							RequireExpirationTime = true,
							ValidateLifetime = true
						};
					});

			return services;
		}
	}
}
