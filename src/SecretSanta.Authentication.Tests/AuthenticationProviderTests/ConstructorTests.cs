using System;
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
    public class ConstructorTests
    {
        [Test]
        public void TestConstructor_ShouldInitializeCorrectly()
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

            // Act
            var provider = new AuthenticationProvider(mockedUserManager.Object, mockedPasswordHasher.Object,
                mockedTokenManager.Object, mockedHttpContextAccessor.Object);

            // Assert
            Assert.IsNotNull(provider);
        }

        [Test]
        public void TestConstructor_PassUserManagerNull_ShouldThrowArgumentNullException()
        {
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

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => new AuthenticationProvider(null, mockedPasswordHasher.Object,
                mockedTokenManager.Object, mockedHttpContextAccessor.Object));
        }

        [Test]
        public void TestConstructor_PassTokenManagerNull_ShouldThrowArgumentNullException()
        {
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
            var mockedHttpContextAccessor = new Mock<IHttpContextAccessor>();

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => new AuthenticationProvider(mockedUserManager.Object, mockedPasswordHasher.Object,
                null, mockedHttpContextAccessor.Object));
        }

        [Test]
        public void TestConstructor_PassPasswordHasherNull_ShouldThrowArgumentNullException()
        {
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

            var mockedTokenManager = new Mock<ITokenManager>();
            var mockedHttpContextAccessor = new Mock<IHttpContextAccessor>();

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => new AuthenticationProvider(mockedUserManager.Object, null,
                mockedTokenManager.Object, mockedHttpContextAccessor.Object));
        }

        [Test]
        public void TestConstructor_PassHttpContextAccessorNull_ShouldThrowArgumentNullException()
        {
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

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => new AuthenticationProvider(mockedUserManager.Object, mockedPasswordHasher.Object,
                mockedTokenManager.Object, null));
        }
    }
}
