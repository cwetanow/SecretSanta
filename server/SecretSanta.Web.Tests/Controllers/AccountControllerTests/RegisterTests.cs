﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SecretSanta.Authentication.Contracts;
using SecretSanta.Common;
using SecretSanta.Factories;
using SecretSanta.Models;
using SecretSanta.Web.Controllers;
using SecretSanta.Web.Infrastructure;
using SecretSanta.Web.Models.Account;
using SecretSanta.Web.Models.Users;

namespace SecretSanta.Web.Tests.Controllers.AccountControllerTests
{
	[TestFixture]
	public class RegisterTests
	{
		[TestCase("username", "email", "name", "password")]
		public async Task TestRegister_ShouldCallAuthenticationProviderFindByUsernameAsyncCorrectly(string username,
			string email,
			string displayName,
			string password)
		{
			// Arrange
			var mockedFactory = new Mock<IUserFactory>();
			var mockedProvider = new Mock<IAuthenticationProvider>();
			var mockedDtoFactory = new Mock<IDtoFactory>();

			var controller = new AccountController(mockedProvider.Object, mockedFactory.Object, mockedDtoFactory.Object);

			var model = new RegisterDto {
				Username = username,
				Email = email,
				DisplayName = displayName,
				Password = password
			};

			// Act
			await controller.Register(model);

			// Assert
			mockedProvider.Verify(p => p.FindByUsernameAsync(username), Times.Once);
		}

		[TestCase("username", "email", "name", "password")]
		public async Task TestRegister_ThereIsUserWithUsername_ShouldReturnBadRequest(string username,
			string email,
			string displayName,
			string password)
		{
			// Arrange
			var mockedFactory = new Mock<IUserFactory>();
			var mockedDtoFactory = new Mock<IDtoFactory>();
			var mockedProvider = new Mock<IAuthenticationProvider>();
			mockedProvider.Setup(p => p.FindByUsernameAsync(It.IsAny<string>())).ReturnsAsync(new User());

			var controller = new AccountController(mockedProvider.Object, mockedFactory.Object, mockedDtoFactory.Object);

			var model = new RegisterDto {
				Username = username,
				Email = email,
				DisplayName = displayName,
				Password = password
			};

			// Act
			var result = await controller.Register(model);

			// Assert
			Assert.IsInstanceOf<BadRequestObjectResult>(result);
		}

		[TestCase("username", "email", "name", "password")]
		public async Task TestRegister_ThereIsUserWithUsername_ShouldReturnBadRequestWithCorrectMessage(string username,
			string email,
			string displayName,
			string password)
		{
			// Arrange
			var mockedFactory = new Mock<IUserFactory>();
			var mockedDtoFactory = new Mock<IDtoFactory>();
			var mockedProvider = new Mock<IAuthenticationProvider>();
			mockedProvider.Setup(p => p.FindByUsernameAsync(It.IsAny<string>())).ReturnsAsync(new User());

			var controller = new AccountController(mockedProvider.Object, mockedFactory.Object, mockedDtoFactory.Object);

			var model = new RegisterDto {
				Username = username,
				Email = email,
				DisplayName = displayName,
				Password = password
			};

			// Act
			var result = await controller.Register(model) as BadRequestObjectResult;

			// Assert
			Assert.AreSame(Constants.UserAlreadyExists, result.Value);
		}

		[TestCase("username", "email", "name", "password")]
		public async Task TestRegister_ThereIsNoUserWithUsername_ShouldCallFactoryCreate(string username,
			string email,
			string displayName,
			string password)
		{
			// Arrange
			var mockedFactory = new Mock<IUserFactory>();
			var mockedProvider = new Mock<IAuthenticationProvider>();
			var mockedDtoFactory = new Mock<IDtoFactory>();

			var controller = new AccountController(mockedProvider.Object, mockedFactory.Object, mockedDtoFactory.Object);

			var model = new RegisterDto {
				Username = username,
				Email = email,
				DisplayName = displayName,
				Password = password
			};

			// Act
			await controller.Register(model);

			// Assert
			mockedFactory.Verify(f => f.CreateUser(username, email, displayName), Times.Once);
		}

		[TestCase("username", "email", "name", "password")]
		public async Task TestRegister_ThereIsNoUserWithUsername_ShouldCallAuthenticationProviderRegisterUserCorrectly(string username,
			string email,
			string displayName,
			string password)
		{
			// Arrange
			var user = new User();

			var mockedFactory = new Mock<IUserFactory>();
			mockedFactory.Setup(f => f.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Returns(user);

			var mockedProvider = new Mock<IAuthenticationProvider>();
			var mockedDtoFactory = new Mock<IDtoFactory>();

			var controller = new AccountController(mockedProvider.Object, mockedFactory.Object, mockedDtoFactory.Object);

			var model = new RegisterDto {
				Username = username,
				Email = email,
				DisplayName = displayName,
				Password = password
			};

			// Act
			await controller.Register(model);

			// Assert
			mockedProvider.Verify(p => p.RegisterUser(user, password), Times.Once);
		}

		[TestCase("username", "email", "name", "password")]
		public async Task TestRegister_AuthenticationProviderRegisterIsNotSuccess_ShouldReturnBadRequest(string username,
			string email,
			string displayName,
			string password)
		{
			// Arrange
			var user = new User();

			var mockedFactory = new Mock<IUserFactory>();
			mockedFactory.Setup(f => f.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Returns(user);

			var mockedProvider = new Mock<IAuthenticationProvider>();
			mockedProvider.Setup(p => p.RegisterUser(It.IsAny<User>(), It.IsAny<string>()))
				.ReturnsAsync(IdentityResult.Failed());

			var mockedDtoFactory = new Mock<IDtoFactory>();

			var controller = new AccountController(mockedProvider.Object, mockedFactory.Object, mockedDtoFactory.Object);

			var model = new RegisterDto {
				Username = username,
				Email = email,
				DisplayName = displayName,
				Password = password
			};

			// Act
			var result = await controller.Register(model);

			// Assert
			Assert.IsInstanceOf<BadRequestResult>(result);
		}

		[TestCase("username", "email", "name", "password")]
		public async Task TestRegister_AuthenticationProviderRegisterIsSuccess_ShouldReturnOk(string username,
			string email,
			string displayName,
			string password)
		{
			// Arrange
			var user = new User();

			var mockedFactory = new Mock<IUserFactory>();
			mockedFactory.Setup(f => f.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Returns(user);

			var mockedProvider = new Mock<IAuthenticationProvider>();
			mockedProvider.Setup(p => p.RegisterUser(It.IsAny<User>(), It.IsAny<string>()))
				.ReturnsAsync(IdentityResult.Success);

			var mockedDtoFactory = new Mock<IDtoFactory>();

			var controller = new AccountController(mockedProvider.Object, mockedFactory.Object, mockedDtoFactory.Object);

			var model = new RegisterDto {
				Username = username,
				Email = email,
				DisplayName = displayName,
				Password = password
			};

			// Act
			var result = await controller.Register(model);

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
		}

		[TestCase("username", "email", "name", "password")]
		public async Task TestRegister_AuthenticationProviderRegisterIsSuccess_ShouldCallDtoFactoryCreate(string username,
			string email,
			string displayName,
			string password)
		{
			// Arrange
			var user = new User { UserName = username, Email = email, DisplayName = displayName };

			var mockedFactory = new Mock<IUserFactory>();
			mockedFactory.Setup(f => f.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Returns(user);

			var mockedProvider = new Mock<IAuthenticationProvider>();
			mockedProvider.Setup(p => p.RegisterUser(It.IsAny<User>(), It.IsAny<string>()))
				.ReturnsAsync(IdentityResult.Success);

			var mockedDtoFactory = new Mock<IDtoFactory>();

			var controller = new AccountController(mockedProvider.Object, mockedFactory.Object, mockedDtoFactory.Object);

			var model = new RegisterDto {
				Username = username,
				Email = email,
				DisplayName = displayName,
				Password = password
			};

			// Act
			await controller.Register(model);

			// Assert
			mockedDtoFactory.Verify(f => f.CreateUserDto(user.UserName, user.Email, user.DisplayName), Times.Once);
		}

		[TestCase("username", "email", "name", "password")]
		public async Task TestRegister_AuthenticationProviderRegisterIsSuccess_ShouldReturnCorrectly(string username,
			string email,
			string displayName,
			string password)
		{
			// Arrange
			var user = new User();

			var mockedFactory = new Mock<IUserFactory>();
			mockedFactory.Setup(f => f.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Returns(user);

			var mockedProvider = new Mock<IAuthenticationProvider>();
			mockedProvider.Setup(p => p.RegisterUser(It.IsAny<User>(), It.IsAny<string>()))
				.ReturnsAsync(IdentityResult.Success);

			var dto = new UserDto();

			var mockedDtoFactory = new Mock<IDtoFactory>();
			mockedDtoFactory.Setup(f => f.CreateUserDto(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Returns(dto);

			var controller = new AccountController(mockedProvider.Object, mockedFactory.Object, mockedDtoFactory.Object);

			var model = new RegisterDto {
				Username = username,
				Email = email,
				DisplayName = displayName,
				Password = password
			};

			// Act
			var result = await controller.Register(model);

			// Assert
			Assert.AreSame(dto, (result as OkObjectResult).Value);
		}
	}
}
