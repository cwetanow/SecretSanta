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
    public class CheckPasswordSignInAsyncTests
    {
        [TestCase("password", "hash")]
        public void TestCheckPasswordSignIn_ShouldCallSignInManagerCheckPasswordSignInAsync(string password, string passHash)
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
            var mockedHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var provider = new AuthenticationProvider(mockedUserManager.Object, mockedPasswordHasher.Object,
                mockedTokenManager.Object, mockedHttpContextAccessor.Object);

            var user = new User { PasswordHash = passHash };

            // Act
            provider.CheckPasswordSignIn(user, password);

            // Assert
            mockedPasswordHasher.Verify(h => h.VerifyHashedPassword(user, passHash, password), Times.Once);
        }

        [TestCase("password")]
        public void TestCheckPasswordSignIn_ShouldReturnCorrectly(string password)
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

            var expectedResult = PasswordVerificationResult.Success;
            mockedPasswordHasher.Setup(h => h.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(PasswordVerificationResult.Success);

            var mockedTokenManager = new Mock<ITokenManager>();
            var mockedHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var provider = new AuthenticationProvider(mockedUserManager.Object, mockedPasswordHasher.Object,
                mockedTokenManager.Object, mockedHttpContextAccessor.Object);

            var user = new User();

            // Act
            var result = provider.CheckPasswordSignIn(user, password);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}
