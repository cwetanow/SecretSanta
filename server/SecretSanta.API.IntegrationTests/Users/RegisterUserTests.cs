using System;
using System.Linq;
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
	public class RegisterUserTests : BaseTestFixture
	{
		public RegisterUserTests(TestWebApplicationFactory factory) : base(factory)
		{ }

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
			var response = await Client.PostAsync("api/users", content);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

		[Theory]
		[InlineData("username", "displayName", "email", "password")]
		public async Task GivenExistingUserUsername_ReturnsBadRequest(string username, string displayName, string email, string password)
		{
			// Arrange
			var user = new User(Guid.NewGuid().ToString(), username, email, displayName);

			var context = this.Factory.GetService<SecretSantaContext>();
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
			var response = await Client.PostAsync("api/users", content);

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
			await Client.PostJson<object>("api/users", body);

			var context = this.Factory.GetService<ApplicationIdentityDbContext>();
			var user = await context.Users.SingleOrDefaultAsync(u => u.UserName == username);

			// Assert
			user.Should().NotBeNull();
			user.UserName.Should().Be(username);
			user.Email.Should().Be(email);
		}

		[Theory]
		[InlineData("username", "displayName", "email@email.com", "!QAZ2wsx")]
		public async Task GivenCorrectParameters_CreatesUser(string username, string displayName, string email, string password)
		{
			// Arrange
			var body = new RegisterUserCommand {
				Username = username,
				DisplayName = displayName,
				Email = email,
				Password = password
			};

			// Act
			await Client.PostJson<object>("api/users", body);

			var identityContext = this.Factory.GetService<ApplicationIdentityDbContext>();
			var identityUserId = await identityContext.Users
				.Where(u => u.UserName == username)
				.Select(u => u.Id)
				.SingleOrDefaultAsync();

			var context = this.Factory.GetService<SecretSantaContext>();
			var user = await context.Users
				.SingleOrDefaultAsync(u => u.Username == username);

			// Assert
			user.Should().NotBeNull();
			user.Username.Should().Be(username);
			user.Email.Should().Be(email);
			user.DisplayName.Should().Be(displayName);
			user.UserId.Should().Be(identityUserId);
		}
	}
}
