using Moq;
using NUnit.Framework;
using SecretSanta.Authentication.Contracts;
using SecretSanta.Services.Contracts;
using SecretSanta.Web.Controllers;
using SecretSanta.Web.Infrastructure;

namespace SecretSanta.Web.Tests.Controllers.InviteControllerTests
{
    [TestFixture]
    public class ConstructorTests
    {
        [Test]
        public void TestConstructor_PassEverything_ShouldInitializeCorrectly()
        {
            // Arrange
            var mockedService = new Mock<IInviteService>();
            var mockedFactory = new Mock<IDtoFactory>();
            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
            var mockedGroupService = new Mock<IGroupService>();

            // Act
            var controller = new InviteController(mockedAuthenticationProvider.Object,
                mockedService.Object, mockedFactory.Object, mockedGroupService.Object);

            // Assert
            Assert.IsNotNull(controller);
        }
    }
}
