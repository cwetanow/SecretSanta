using System;
using Moq;
using NUnit.Framework;
using SecretSanta.Authentication.Contracts;
using SecretSanta.Factories;
using SecretSanta.Web.Controllers;
using SecretSanta.Web.Infrastructure;

namespace SecretSanta.Web.Tests.Controllers.AccountControllerTests
{
    [TestFixture]
    public class ConstructorTests
    {
        [Test]
        public void TestConstructor_ShouldInitializeCorrectly()
        {
            // Arrange
            var mockedFactory = new Mock<IUserFactory>();
            var mockedProvider = new Mock<IAuthenticationProvider>();
            var mockedDtoFactory = new Mock<IDtoFactory>();

            // Act
            var controller = new AccountController(mockedProvider.Object, mockedFactory.Object, mockedDtoFactory.Object);

            // Assert
            Assert.IsNotNull(controller);
        }

        [Test]
        public void TestConstructor_PassAuthenticationProviderNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var mockedFactory = new Mock<IUserFactory>();
            var mockedDtoFactory = new Mock<IDtoFactory>();

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => new AccountController(null, mockedFactory.Object, mockedDtoFactory.Object));
        }

        [Test]
        public void TestConstructor_PassUserFactoryNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var mockedProvider = new Mock<IAuthenticationProvider>();
            var mockedDtoFactory = new Mock<IDtoFactory>();

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => new AccountController(mockedProvider.Object, null, mockedDtoFactory.Object));
        }

        [Test]
        public void TestConstructor_PassDtoFactoryNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var mockedProvider = new Mock<IAuthenticationProvider>();
            var mockedFactory = new Mock<IUserFactory>();

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => new AccountController(mockedProvider.Object, mockedFactory.Object, null));
        }
    }
}
