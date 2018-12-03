using Moq;
using NUnit.Framework;
using SecretSanta.Authentication.Contracts;
using SecretSanta.Services.Contracts;
using SecretSanta.Web.Controllers;
using SecretSanta.Web.Infrastructure;
using System;

namespace SecretSanta.Web.Tests.Controllers.GroupControllerTests
{
	[TestFixture]
	public class ConstructorTests
	{
		[Test]
		public void TestConstructor_PassEverything_ShouldInitializeCorrectly()
		{
			// Arrange
			var mockedService = new Mock<IGroupService>();
			var mockedFactory = new Mock<IDtoFactory>();
			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			var mockedMembershipService = new Mock<IMembershipService>();

			// Act
			var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object, mockedMembershipService.Object);

			// Assert
			Assert.IsNotNull(controller);
		}

		[Test]
		public void TestConstructor_PassServiceNull_ShouldThrowArgumentNullException()
		{
			// Arrange
			var mockedFactory = new Mock<IDtoFactory>();
			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			var mockedMembershipService = new Mock<IMembershipService>();

			// Act, Assert
			Assert.Throws<ArgumentNullException>(() => new GroupController(null, mockedFactory.Object, mockedAuthenticationProvider.Object, mockedMembershipService.Object));
		}

		[Test]
		public void TestConstructor_PassFactoryNull_ShouldThrowArgumentNullException()
		{
			// Arrange
			var mockedService = new Mock<IGroupService>();
			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			var mockedMembershipService = new Mock<IMembershipService>();

			// Act, Assert
			Assert.Throws<ArgumentNullException>(() => new GroupController(mockedService.Object, null, mockedAuthenticationProvider.Object, mockedMembershipService.Object));
		}

		[Test]
		public void TestConstructor_PassAuthenticationProviderNull_ShouldThrowArgumentNullException()
		{
			// Arrange
			var mockedService = new Mock<IGroupService>();
			var mockedFactory = new Mock<IDtoFactory>();
			var mockedMembershipService = new Mock<IMembershipService>();

			// Act, Assert
			Assert.Throws<ArgumentNullException>(() => new GroupController(mockedService.Object, mockedFactory.Object, null, mockedMembershipService.Object));
		}
	}
}
