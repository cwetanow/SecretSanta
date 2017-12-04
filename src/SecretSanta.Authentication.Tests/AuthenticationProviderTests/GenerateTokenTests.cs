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

            var mockedAccessor = new HttpContextAccessor();
            var mockedUserClaimsPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            var mockedIdentityOptions = new Mock<IOptions<IdentityOptions>>();
            var mockedSignInLogger = new Mock<ILogger<SignInManager<User>>>();

            var mockedSchemeProvider = new Mock<IAuthenticationSchemeProvider>();

            var mockedSignInManager = new Mock<SignInManager<User>>(mockedUserManager.Object, mockedAccessor,
                mockedUserClaimsPrincipalFactory.Object, mockedIdentityOptions.Object, mockedSignInLogger.Object,
                mockedSchemeProvider.Object);

            var mockedTokenManager = new Mock<ITokenManager>();

            var provider = new AuthenticationProvider(mockedUserManager.Object, mockedSignInManager.Object,
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

            var mockedAccessor = new HttpContextAccessor();
            var mockedUserClaimsPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            var mockedIdentityOptions = new Mock<IOptions<IdentityOptions>>();
            var mockedSignInLogger = new Mock<ILogger<SignInManager<User>>>();

            var mockedSchemeProvider = new Mock<IAuthenticationSchemeProvider>();

            var mockedSignInManager = new Mock<SignInManager<User>>(mockedUserManager.Object, mockedAccessor,
                mockedUserClaimsPrincipalFactory.Object, mockedIdentityOptions.Object, mockedSignInLogger.Object,
                mockedSchemeProvider.Object);

            var expectedResult = SignInResult.Success;
            mockedSignInManager.Setup(m => m.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(expectedResult);

            var mockedTokenManager = new Mock<ITokenManager>();
            mockedTokenManager.Setup(m => m.GenerateToken(It.IsAny<string>())).Returns(token);

            var provider = new AuthenticationProvider(mockedUserManager.Object, mockedSignInManager.Object,
                mockedTokenManager.Object);

            // Act
            var result = provider.GenerateToken(email);

            // Assert
            Assert.AreSame(token, result);
        }
    }
}
