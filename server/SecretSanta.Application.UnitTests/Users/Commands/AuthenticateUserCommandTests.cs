using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using SecretSanta.Application.Common.Exceptions;
using SecretSanta.Application.Common.Interfaces;
using SecretSanta.Application.Common.Models;
using SecretSanta.Application.UnitTests.Common;
using SecretSanta.Application.Users.Commands;
using SecretSanta.Application.Users.Responses;
using SecretSanta.Domain.Entities;
using Xunit;

namespace SecretSanta.Application.UnitTests.Users.Commands
{
	public class AuthenticateUserCommandTests : BaseTestFixture
	{
		[Fact]
		public async Task GivenIdentityServiceReturnsFailure_ThrowsBadRequestException()
		{
			// Arrange
			var identityServiceMock = new Mock<IIdentityService>();
			identityServiceMock
				.Setup(s => s.AuthenticateUser(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(Result.CreateFailure());

			var command = new AuthenticateUserCommand();

			var sut = new AuthenticateUserCommand.Handler(identityServiceMock.Object, Context, Mapper);

			// Act
			var action = new Func<Task<UserProfileResponse>>(() => sut.Handle(command, CancellationToken.None));

			// Assert
			await action.Should().ThrowAsync<BadRequestException>();
		}

		[Fact]
		public async Task GivenUserDoesntExist_ThrowsEntityNotFoundException()
		{
			// Arrange
			var identityServiceMock = new Mock<IIdentityService>();
			identityServiceMock
				.Setup(s => s.AuthenticateUser(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(Result.CreateSuccess());

			var command = new AuthenticateUserCommand();

			var sut = new AuthenticateUserCommand.Handler(identityServiceMock.Object, Context, Mapper);

			// Act
			var action = new Func<Task<UserProfileResponse>>(() => sut.Handle(command, CancellationToken.None));

			// Assert
			await action.Should().ThrowAsync<EntityNotFoundException<User>>();
		}

		[Fact]
		public async Task GivenUserExists_ReturnsCorrectly()
		{
			// Arrange
			var user = new User(Guid.NewGuid().ToString(), "username", "email", "display name");
			Context.Users.Add(user);
			await Context.SaveChangesAsync();

			var identityServiceMock = new Mock<IIdentityService>();
			identityServiceMock
				.Setup(s => s.AuthenticateUser(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(Result.CreateSuccess());

			var command = new AuthenticateUserCommand {
				Username = user.Username,
				Password = Guid.NewGuid().ToString()
			};

			var sut = new AuthenticateUserCommand.Handler(identityServiceMock.Object, Context, Mapper);

			// Act
			var response = await sut.Handle(command, CancellationToken.None);

			// Assert
			response.Email.Should().Be(user.Email);
			response.Username.Should().Be(user.Username);
			response.DisplayName.Should().Be(user.DisplayName);
			response.Id.Should().Be(user.Id);
		}
	}
}
