using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoBogus;
using Bogus;
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

		[Fact]
		public async Task GivenCorrectParameters_CreatesIdentityUser()
		{
			// Arrange
			var body = new Faker<RegisterUserCommand>()
				.RuleFor(c => c.Username, f => f.Internet.UserName())
				.RuleFor(c => c.DisplayName, f => f.Person.FullName)
				.RuleFor(c => c.Email, f => f.Person.Email)
				.RuleFor(c => c.Password, "^YHN7ujm")
				.Generate();

			// Act
			await Client.PostJson<object>("api/users", body);

			var context = this.Factory.GetService<ApplicationIdentityDbContext>();
			var user = await context.Users.SingleOrDefaultAsync(u => u.UserName == body.Username);

			// Assert
			user.Should().NotBeNull();
			user.UserName.Should().Be(body.Username);
			user.Email.Should().Be(body.Email);
		}

		[Fact]
		public async Task GivenCorrectParameters_CreatesUser()
		{
			// Arrange
			var body = new Faker<RegisterUserCommand>()
				.RuleFor(c => c.Username, f => f.Internet.UserName())
				.RuleFor(c => c.DisplayName, f => f.Person.FullName)
				.RuleFor(c => c.Email, f => f.Person.Email)
				.RuleFor(c => c.Password, "^YHN7ujm")
				.Generate();

			// Act
			var userId = await Client.PostJson<int>("api/users", body);

			var identityContext = this.Factory.GetService<ApplicationIdentityDbContext>();
			var identityUserId = await identityContext.Users
				.Where(u => u.UserName == body.Username)
				.Select(u => u.Id)
				.SingleOrDefaultAsync();

			var context = this.Factory.GetService<SecretSantaContext>();
			var user = await context.Users
				.SingleOrDefaultAsync(u => u.Id == userId);

			// Assert
			user.Should().NotBeNull();
			user.Username.Should().Be(body.Username);
			user.Email.Should().Be(body.Email);
			user.DisplayName.Should().Be(body.DisplayName);
			user.UserId.Should().Be(identityUserId);
		}
	}
}
