using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using SecretSanta.Application.UnitTests.Common;
using SecretSanta.Application.Users.Queries;
using SecretSanta.Application.Users.Responses;
using SecretSanta.Domain.Entities;
using Xunit;

namespace SecretSanta.Application.UnitTests.Users.Queries
{
	public class UserListQueryTests : BaseTestFixture
	{
		[Fact]
		public async Task GivenNoSearchPattern_ShouldReturnCorrectly()
		{
			// Arrange
			var users = new List<User>
			{
				new User(Guid.NewGuid().ToString(),"username1","email1","displayName1"),
				new User(Guid.NewGuid().ToString(),"username2","email2","displayName2")
			};

			Context.Users.AddRange(users);
			await Context.SaveChangesAsync();

			var expected = users
				.Select(u => new UserProfileResponse {
					DisplayName = u.DisplayName,
					Email = u.Email,
					Username = u.Username,
					Id = u.Id
				});

			var request = new UserListQuery();

			var sut = new UserListQuery.Handler(Context, Mapper);

			// Act
			var result = await sut.Handle(request, CancellationToken.None);

			// Assert
			result.Users.Should().BeEquivalentTo(expected);
		}

		[Fact]
		public async Task GivenSearchPattern_ShouldFilterCorrectly()
		{
			// Arrange
			var users = new List<User>
			{
				new User(Guid.NewGuid().ToString(),"usernameWithSearchPattern","email1","displayName1"),
				new User(Guid.NewGuid().ToString(),"username2","email2","displayNameWithSearchPattern"),
				new User(Guid.NewGuid().ToString(),"usernameWithoutSearchPattern","email2","displayNameWithoutSearchPattern")
			};

			Context.Users.AddRange(users);
			await Context.SaveChangesAsync();

			var expected = users
				.Take(2)
				.Select(u => new UserProfileResponse {
					DisplayName = u.DisplayName,
					Email = u.Email,
					Username = u.Username,
					Id = u.Id
				});

			var request = new UserListQuery {
				SearchPattern = "WithSearchPattern"
			};

			var sut = new UserListQuery.Handler(Context, Mapper);

			// Act
			var result = await sut.Handle(request, CancellationToken.None);

			// Assert
			result.Users.Should().BeEquivalentTo(expected);
		}

		[Fact]
		public async Task GivenDescendingOrder_ShouldReturnCorrectly()
		{
			// Arrange
			var users = new List<User>
			{
				new User(Guid.NewGuid().ToString(),"username1","email1","displayName1"),
				new User(Guid.NewGuid().ToString(),"username2","email2","displayName2")
			};

			Context.Users.AddRange(users);
			await Context.SaveChangesAsync();

			var request = new UserListQuery {
				SortAscending = false
			};

			var sut = new UserListQuery.Handler(Context, Mapper);

			// Act
			var result = await sut.Handle(request, CancellationToken.None);

			// Assert
			result.Users.Should().BeInDescendingOrder(r => r.Username);
		}

		[Fact]
		public async Task GivenLimitAndOffset_ShouldReturnCorrectly()
		{
			// Arrange
			var users = new List<User>
			{
				new User(Guid.NewGuid().ToString(),"username1","email1","displayName1"),
				new User(Guid.NewGuid().ToString(),"username2","email2","displayName2"),
				new User(Guid.NewGuid().ToString(),"username3","email3","displayName3")
			};

			Context.Users.AddRange(users);
			await Context.SaveChangesAsync();

			var expected = new List<User> { users[1] }
				.Select(u => new UserProfileResponse {
					DisplayName = u.DisplayName,
					Email = u.Email,
					Username = u.Username,
					Id = u.Id
				});

			var request = new UserListQuery {
				Offset = 1,
				Limit = 1
			};

			var sut = new UserListQuery.Handler(Context, Mapper);

			// Act
			var result = await sut.Handle(request, CancellationToken.None);

			// Assert
			result.Users.Should().BeEquivalentTo(expected);
		}
	}
}
