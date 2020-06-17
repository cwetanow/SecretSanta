using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SecretSanta.Application.Common.Exceptions;
using SecretSanta.Application.Common.Interfaces;
using SecretSanta.Application.Common.Models;
using SecretSanta.Application.UnitTests.Common;
using SecretSanta.Application.Users.Commands;
using SecretSanta.Domain.Entities;
using Xunit;

namespace SecretSanta.Application.UnitTests.Users.Commands
{
	public class RegisterUserCommandTests : BaseTestFixture
	{
		[Fact]
		public async Task GivenExistingUser_ThrowsBadRequestExceptionWithMessage()
		{
			// Arrange
			var command = new RegisterUserCommand {
				Email = "Email",
				Password = "Password",
				Username = "Username",
				DisplayName = "DisplayName"
			};

			var userId = Guid.NewGuid().ToString();

			var existingUser = new User(userId, command.Username, command.Email, command.DisplayName);
			Context.Users.Add(existingUser);
			await Context.SaveChangesAsync();

			var userServiceMock = new Mock<IUserService>();

			var sut = new RegisterUserCommand.Handler(userServiceMock.Object, Context);

			// Act
			var action = new Func<Task<int>>(() => sut.Handle(command, CancellationToken.None));

			// Assert
			(await action.Should().ThrowAsync<BadRequestException>())
				.And.Errors.Should().Contain(err => err.Contains(command.Username));
		}

		[Fact]
		public async Task GivenNotSuccessWhenCreatingUser_ThrowsBadRequestException()
		{
			// Arrange
			var command = new RegisterUserCommand {
				Email = "Email",
				Password = "Password",
				Username = "Username"
			};

			var errors = new[] { "error", "other error" };

			var userServiceMock = new Mock<IUserService>();
			userServiceMock.Setup(s => s.CreateUser(command.Username, command.Email, command.Password))
				.ReturnsAsync((Result.CreateFailure(errors), string.Empty));

			var sut = new RegisterUserCommand.Handler(userServiceMock.Object, Context);

			// Act
			var action = new Func<Task<int>>(() => sut.Handle(command, CancellationToken.None));

			// Assert
			(await action.Should().ThrowAsync<BadRequestException>())
				.And.Errors.Should().BeEquivalentTo(errors);
		}

		[Fact]
		public async Task GivenSuccessWhenCreatingUser_CreatesUserCorrectly()
		{
			// Arrange
			var command = new RegisterUserCommand {
				Email = "Email",
				Password = "Password",
				Username = "Username",
				DisplayName = "DisplayName"
			};

			var userId = Guid.NewGuid().ToString();

			var userServiceMock = new Mock<IUserService>();
			userServiceMock.Setup(s => s.CreateUser(command.Username, command.Email, command.Password))
				.ReturnsAsync((Result.CreateSuccess(), userId));

			var sut = new RegisterUserCommand.Handler(userServiceMock.Object, Context);

			// Act
			var id = await sut.Handle(command, CancellationToken.None);

			var user = await Context.Users.SingleOrDefaultAsync(u => u.Id == id);

			// Assert
			user.Should().NotBeNull();
			user.Username.Should().Be(command.Username);
			user.DisplayName.Should().Be(command.DisplayName);
			user.Email.Should().Be(command.Email);
			user.UserId.Should().Be(userId);
		}
	}
}
