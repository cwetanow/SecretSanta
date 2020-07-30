using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using SecretSanta.API.IntegrationTests.Common;
using SecretSanta.API.IntegrationTests.Common.Extensions;
using SecretSanta.Application.Users.Responses;
using SecretSanta.Domain.Entities;
using SecretSanta.Persistence;
using Xunit;

namespace SecretSanta.API.IntegrationTests.Users
{
	public class GetUserByUsernameTests : BaseTestFixture
	{
		public GetUserByUsernameTests(TestWebApplicationFactory factory) : base(factory)
		{ }

		[Fact]
		public async Task GivenNonExistingUser_ReturnsNotFound()
		{
			// Arrange
			var nonExistingUsername = "nonExistingUsername";

			var client = await Factory.CreateAuthenticatedClientAsync();

			// Act
			var response = await client.GetAsync($"api/users/{nonExistingUsername}");

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}

		[Fact]
		public async Task GivenExistingUsername_ReturnsUser()
		{
			// Arrange
			var user = new User(Guid.NewGuid().ToString(), "username", "email", "displayName");

			var context = Factory.GetService<SecretSantaContext>();
			context.Users.Add(user);
			await context.SaveChangesAsync();

			var client = await Factory.CreateAuthenticatedClientAsync();

			// Act
			var response = await client.GetJsonAsync<UserProfileResponse>($"api/users/{user.Username}");

			// Assert
			response.Should().BeEquivalentTo(user,
				opts => opts.Excluding(u => u.UserId));
		}
	}
}
