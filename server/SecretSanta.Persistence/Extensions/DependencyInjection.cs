using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SecretSanta.Persistence.Extensions
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddPersistence(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction)
		{
			services
				.AddDbContext<SecretSantaContext>(optionsAction)
				.AddScoped<DbContext>(provider => provider.GetService<SecretSantaContext>());

			return services;
		}
	}
}
