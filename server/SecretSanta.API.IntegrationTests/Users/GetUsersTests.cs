using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SecretSanta.API.IntegrationTests.Common;
using SecretSanta.API.IntegrationTests.Common.Extensions;
using SecretSanta.Application.Users.Responses;
using SecretSanta.Domain.Entities;
using SecretSanta.Persistence;
using Xunit;

namespace SecretSanta.API.IntegrationTests.Users
{
	public class GetUsersTests : BaseTestFixture
	{
		public GetUsersTests(TestWebApplicationFactory factory) : base(factory)
		{ }

		[Fact]
		public async Task ReturnListOfUsers()
		{
			// Arrange
			var users = new List<User>
			{
				new User(Guid.NewGuid().ToString(),"username1","email1","displayName1"),
				new User(Guid.NewGuid().ToString(),"username2","email2","displayName2")
			};

			var context = Factory.GetService<SecretSantaContext>();

			context.Users.AddRange(users);
			await context.SaveChangesAsync();

			var client = await Factory.CreateAuthenticatedClientAsync();

			var expected = await context.Users.ToListAsync();

			// Act
			var response = await client.GetJsonAsync<UserListResponse>("api/users");

			// Assert
			response.Users.Should().BeEquivalentTo(expected,
				opts => opts.Excluding(u => u.UserId));
		}
	}
}
