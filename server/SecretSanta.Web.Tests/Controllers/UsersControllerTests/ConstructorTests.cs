using System;
using Moq;
using NUnit.Framework;
using SecretSanta.Authentication.Contracts;
using SecretSanta.Factories;
using SecretSanta.Services.Contracts;
using SecretSanta.Web.Controllers;
using SecretSanta.Web.Infrastructure;

namespace SecretSanta.Web.Tests.Controllers.UsersControllerTests
{
    [TestFixture]
    public class ConstructorTests
    {
        [Test]
        public void TestConstructor_ShouldInitializeCorrectly()
        {
            // Arrange
            var mockedDtoFactory = new Mock<IDtoFactory>();
            var mockedService = new Mock<IUserService>();

            // Act
            var controller = new UsersController(mockedService.Object, mockedDtoFactory.Object);

            // Assert
            Assert.IsNotNull(controller);
        }

        [Test]
        public void TestConstructor_PassUserServiceNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var mockedDtoFactory = new Mock<IDtoFactory>();
            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => new UsersController(null, mockedDtoFactory.Object));
        }

        [Test]
        public void TestConstructor_PassDtoFactoryNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var mockedService = new Mock<IUserService>();

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => new UsersController(mockedService.Object, null));
        }
    }
}
