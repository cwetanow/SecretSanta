using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SecretSanta.Identity;
using SecretSanta.Persistence;

namespace SecretSanta.API.IntegrationTests.Common
{
	public class TestWebApplicationFactory : WebApplicationFactory<Startup>
	{
		private SqliteConnection connection;

		public TService GetService<TService>()
		{
			var scope = this.Services.CreateScope();
			return scope.ServiceProvider.GetRequiredService<TService>();
		}

		public void CleanupDatabase()
		{
			var scope = this.Services.CreateScope();

			var context = scope.ServiceProvider.GetRequiredService<SecretSantaContext>();
			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();

			var identityContext = scope.ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>();
			identityContext.Database.EnsureDeleted();
			identityContext.Database.EnsureCreated();
		}

		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			builder.ConfigureServices(ConfigureTestServices);

			base.ConfigureWebHost(builder);
		}

		private void ConfigureTestServices(IServiceCollection services)
		{
			var dbContextDescriptors = services
				.Where(d => d.ServiceType == typeof(DbContextOptions<>));

			foreach (var descriptor in dbContextDescriptors)
			{
				services.Remove(descriptor);
			}

			connection = new SqliteConnection("DataSource=:memory:");
			connection.Open();

			var identityDbName = Guid.NewGuid().ToString();

			services
				.AddDbContext<SecretSantaContext>(opts => opts.UseSqlite(connection))
				.AddDbContext<ApplicationIdentityDbContext>(opts => opts.UseInMemoryDatabase(identityDbName));

			var provider = services.BuildServiceProvider();

			using var scope = provider.CreateScope();

			var context = scope.ServiceProvider.GetRequiredService<SecretSantaContext>();
			context.Database.EnsureCreated();

			var identityContext = scope.ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>();
			identityContext.Database.EnsureCreated();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				connection.Close();
			}

			base.Dispose(disposing);
		}
	}
}
