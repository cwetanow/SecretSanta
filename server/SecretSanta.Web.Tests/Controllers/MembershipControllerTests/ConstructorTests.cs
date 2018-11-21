using Moq;
using NUnit.Framework;
using SecretSanta.Authentication.Contracts;
using SecretSanta.Services.Contracts;
using SecretSanta.Web.Controllers;

namespace SecretSanta.Web.Tests.Controllers.MembershipControllerTests
{
    [TestFixture]
    public class ConstructorTests
    {
        [Test]
        public void TestConstructor_ShouldInitializeCorrectly()
        {
            // Arrange
            var mockedMembershipService = new Mock<IMembershipService>();
            var mockedInviteService = new Mock<IInviteService>();
            var mockedGroupService = new Mock<IGroupService>();
            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();

            // Act
            var controller = new MembershipController(mockedMembershipService.Object, mockedInviteService.Object, mockedGroupService.Object,
                mockedAuthenticationProvider.Object);

            // Assert
            Assert.IsNotNull(controller);
        }
    }
}
