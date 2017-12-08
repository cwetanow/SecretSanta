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
    public class GenerateTokenTests
    {
        [TestCase("email")]
        public void TestGenerateToken_ShouldCallTokenManagerGenerateToken(string email)
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

            // Act
            provider.GenerateToken(email);

            // Assert
            mockedTokenManager.Verify(m => m.GenerateToken(email), Times.Once);
        }

        [TestCase("email", "token")]
        public void TestGenerateToken_ShouldReturnCorrectly(string email, string token)
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
            mockedPasswordHasher.Setup(m => m.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(expectedResult);

            var mockedTokenManager = new Mock<ITokenManager>();
            mockedTokenManager.Setup(m => m.GenerateToken(It.IsAny<string>())).Returns(token);

            var provider = new AuthenticationProvider(mockedUserManager.Object, mockedPasswordHasher.Object,
                mockedTokenManager.Object);

            // Act
            var result = provider.GenerateToken(email);

            // Assert
            Assert.AreSame(token, result);
        }
    }
}
