using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SecretSanta.API.IntegrationTests.Common;
using SecretSanta.API.IntegrationTests.Common.Extensions;
using SecretSanta.Application.Users.Commands;
using SecretSanta.Domain.Entities;
using SecretSanta.Identity;
using SecretSanta.Persistence;
using Xunit;

namespace SecretSanta.API.IntegrationTests.Users
{
	public class RegisterUserTests : IClassFixture<TestWebApplicationFactory>
	{
		private readonly TestWebApplicationFactory factory;
		private readonly HttpClient client;

		public RegisterUserTests(TestWebApplicationFactory factory)
		{
			this.factory = factory;
			factory.CleanupDatabase();

			this.client = factory.CreateClient();
		}

		[Theory]
		[InlineData("", "displayName", "email", "password")]
		[InlineData("username", "", "email", "password")]
		[InlineData("username", "displayName", "", "password")]
		[InlineData("username", "displayName", "email", "")]
		public async Task GivenInvalidParameters_ReturnsBadRequest(string username, string displayName, string email, string password)
		{
			// Arrange
			var body = new RegisterUserCommand {
				Username = username,
				DisplayName = displayName,
				Email = email,
				Password = password
			};

			var content = new StringContent(JsonConvert.SerializeObject(body));
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			// Act
			var response = await client.PostAsync("api/users", content);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

		[Theory]
		[InlineData("username", "displayName", "email", "password")]
		public async Task GivenExistingUserUsername_ReturnsBadRequest(string username, string displayName, string email, string password)
		{
			// Arrange
			var user = new User(Guid.NewGuid().ToString(), username, email, displayName);

			var context = this.factory.GetService<SecretSantaContext>();
			context.Users.Add(user);
			await context.SaveChangesAsync();

			var body = new RegisterUserCommand {
				Username = username,
				DisplayName = displayName,
				Email = email,
				Password = password
			};

			var content = new StringContent(JsonConvert.SerializeObject(body));
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			// Act
			var response = await client.PostAsync("api/users", content);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

		[Theory]
		[InlineData("username", "displayName", "email@email.com", "!QAZ2wsx")]
		public async Task GivenCorrectParameters_CreatesIdentityUser(string username, string displayName, string email, string password)
		{
			// Arrange
			var body = new RegisterUserCommand {
				Username = username,
				DisplayName = displayName,
				Email = email,
				Password = password
			};

			// Act
			await client.PostJson<object>("api/users", body);

			var context = this.factory.GetService<ApplicationIdentityDbContext>();
			var user = await context.Users.SingleOrDefaultAsync(u => u.UserName == username);

			// Assert
			user.Should().NotBeNull();
			user.UserName.Should().Be(username);
			user.Email.Should().Be(email);
		}
	}
}
