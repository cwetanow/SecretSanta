using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SecretSanta.Authentication.Contracts;
using SecretSanta.Models;

namespace SecretSanta.Authentication.Tests.AuthenticationProviderTests
{
    [TestFixture]
    public class RegisterUserTests
    {
        [TestCase("hashed")]
        [TestCase("password")]
        public void TestRegisterUser_ShouldCallUserManagerCreateAsync(string password)
        {
            // Arrange
            var mockedUserStore = new Mock<IUserStore<User>>();
            var mockedOptions = new Mock<IOptions<IdentityOptions>>();
            var mockedHasher = new Mock<IPasswordHasher<User>>();
            var mockedUserValidator = new IUserValidator<User>[0];
            var mockedPasswordValidator = new IPasswordValidator<User>[0];
            var mockedNormalizer = new Mock<ILookupNormalizer>();
            var mockedDescriber = new Mock<IdentityErrorDescriber>();
            var mockedProvider = new Mock<IServiceProvider>();
            var mockedLogger = new Mock<ILogger<UserManager<User>>>();

            var mockedUserManager = new Mock<UserManager<User>>(mockedUserStore.Object, mockedOptions.Object, mockedHasher.Object,
                mockedUserValidator, mockedPasswordValidator, mockedNormalizer.Object, mockedDescriber.Object,
                mockedProvider.Object, mockedLogger.Object);

            var mockedPasswordHasher = new Mock<IPasswordHasher<User>>();

            var mockedTokenManager = new Mock<ITokenManager>();

            var provider = new AuthenticationProvider(mockedUserManager.Object, mockedPasswordHasher.Object,
                mockedTokenManager.Object);

            var user = new User();

            // Act
            provider.RegisterUser(user, password);

            // Assert
            mockedUserManager.Verify(u => u.CreateAsync(user, password), Times.Once);
        }

        [TestCase("hashed")]
        [TestCase("password")]
        public async Task TestRegisterUser_ShouldReturnCorrectly(string password)
        {
            // Arrange
            var mockedUserStore = new Mock<IUserStore<User>>();
            var mockedOptions = new Mock<IOptions<IdentityOptions>>();
            var mockedHasher = new Mock<IPasswordHasher<User>>();
            var mockedUserValidator = new IUserValidator<User>[0];
            var mockedPasswordValidator = new IPasswordValidator<User>[0];
            var mockedNormalizer = new Mock<ILookupNormalizer>();
            var mockedDescriber = new Mock<IdentityErrorDescriber>();
            var mockedProvider = new Mock<IServiceProvider>();
            var mockedLogger = new Mock<ILogger<UserManager<User>>>();

            var mockedUserManager = new Mock<UserManager<User>>(mockedUserStore.Object, mockedOptions.Object, mockedHasher.Object,
                mockedUserValidator, mockedPasswordValidator, mockedNormalizer.Object, mockedDescriber.Object,
                mockedProvider.Object, mockedLogger.Object);

            var user = new User();
            var expectedResult = IdentityResult.Success;

            mockedUserManager.Setup(u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(expectedResult);

            var mockedPasswordHasher = new Mock<IPasswordHasher<User>>();

            var mockedTokenManager = new Mock<ITokenManager>();

            var provider = new AuthenticationProvider(mockedUserManager.Object, mockedPasswordHasher.Object,
                mockedTokenManager.Object);

            // Act
            var result = await provider.RegisterUser(user, password);

            // Assert
            Assert.AreSame(expectedResult, result);
        }
    }
}
