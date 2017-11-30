using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SecretSanta.Models;

namespace SecretSanta.Authentication.Tests.AuthenticationProviderTests
{
    [TestFixture]
    public class FindByUsernameAsyncTests
    {
        [TestCase("username")]
        [TestCase("test")]
        public void TestFindByUsername_ShouldCallUserManagerFindByNameAsync(string username)
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

            var provider = new AuthenticationProvider(mockedUserManager.Object);

            // Act
            provider.FindByUsernameAsync(username);

            // Assert
            mockedUserManager.Verify(u => u.FindByNameAsync(username), Times.Once);
        }

        [TestCase("username")]
        [TestCase("test")]
        public async Task TestFindByUsername_ShouldReturnCorrectly(string username)
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

            mockedUserManager.Setup(u => u.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            var provider = new AuthenticationProvider(mockedUserManager.Object);

            // Act
            var result = await provider.FindByUsernameAsync(username);

            // Assert
            Assert.AreSame(user, result);
        }
    }
}
