using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using SecretSanta.Application.Common.Exceptions;
using SecretSanta.Application.UnitTests.Common;
using SecretSanta.Application.Users.Queries;
using SecretSanta.Application.Users.Responses;
using SecretSanta.Domain.Entities;
using Xunit;

namespace SecretSanta.Application.UnitTests.Users.Queries
{
	public class UserByUsernameQueryTests : BaseTestFixture
	{
		[Fact]
		public async Task GivenNonExistingUsername_ThrowsEntityNotFoundException()
		{
			// Arrange
			var request = new UserByUsernameQuery {
				Username = "someusername"
			};

			var sut = new UserByUsernameQuery.Handler(Context, Mapper);

			// Act
			var action = new Func<Task<UserProfileResponse>>(() => sut.Handle(request, CancellationToken.None));

			// Assert
			await action.Should().ThrowAsync<EntityNotFoundException>();
		}

		[Fact]
		public async Task GivenExistingUsername_ReturnsUser()
		{
			// Arrange
			var user = new User(Guid.NewGuid().ToString(), "username", "email", "displayName");

			Context.Users.Add(user);
			await Context.SaveChangesAsync();

			var request = new UserByUsernameQuery {
				Username = user.Username
			};

			var sut = new UserByUsernameQuery.Handler(Context, Mapper);

			// Act
			var result = await sut.Handle(request, CancellationToken.None);

			// Assert
			result.Should().BeEquivalentTo(user,
				opts => opts.Excluding(u => u.UserId));
		}
	}
}
