using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SecretSanta.API.IntegrationTests.Common.Extensions;
using SecretSanta.API.Models.Responses;
using SecretSanta.Application.Users.Commands;
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

		public async Task<HttpClient> CreateAuthenticatedClientAsync()
		{
			var password = "^YHN7ujm";

			var scope = this.Services.CreateScope();

			var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
			await mediator.Send(new RegisterUserCommand {
				Email = AuthenticatedUser.Email,
				Password = password,
				Username = AuthenticatedUser.UserName,
				DisplayName = "display name"
			});


			var authRequest = new AuthenticateUserCommand { Password = password, Username = AuthenticatedUser.UserName };
			var client = CreateClient();
			var response = await client.PostJson<LoginResponse>("api/login", authRequest);

			client.DefaultRequestHeaders.Add("Authorization", $"Bearer {response.Token}");

			return client;
		}

		public ApplicationUser AuthenticatedUser = new ApplicationUser("testuser", "testuser@secret.santa");

		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			builder.ConfigureServices(ConfigureTestServices);

			base.ConfigureWebHost(builder);
		}

		private void ConfigureTestServices(IServiceCollection services)
		{
			var dbContextDescriptors = services
				.Where(d => d.ServiceType == typeof(DbContextOptions<SecretSantaContext>) ||
							d.ServiceType == typeof(DbContextOptions<ApplicationIdentityDbContext>))
				.ToList();

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
