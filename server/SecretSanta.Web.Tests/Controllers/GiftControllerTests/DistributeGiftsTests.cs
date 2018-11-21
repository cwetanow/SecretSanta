using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SecretSanta.Authentication.Contracts;
using SecretSanta.Models;
using SecretSanta.Services.Contracts;
using SecretSanta.Web.Controllers;
using SecretSanta.Web.Infrastructure;
using SecretSanta.Web.Models.Gift;
using SecretSanta.Web.Models.Group;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecretSanta.Web.Tests.Controllers.GiftControllerTests
{
	[TestFixture]
	public class DistributeGiftsTests
	{
		[TestCase("group", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
		[TestCase("my group", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
		public async Task TestDistributeGifts_ShouldCallGetCurrentUserAsync(string groupName, string userId)
		{
			// Arrange
			var mockedService = new Mock<IGiftService>();
			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			var mockedFactory = new Mock<IDtoFactory>();
			var mockedGroupService = new Mock<IGroupService>();
			var mockedInviteService = new Mock<IInviteService>();

			var controller = new GiftController(mockedService.Object, mockedAuthenticationProvider.Object, mockedFactory.Object, mockedGroupService.Object, mockedInviteService.Object);

			// Act
			await controller.DistributeGifts(groupName);

			// Assert
			mockedAuthenticationProvider.Verify(p => p.GetCurrentUserAsync(), Times.Once);
		}

		[TestCase("group", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
		[TestCase("my group", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
		public async Task TestDistributeGifts_ShouldCallGroupServiceGetByName(string groupName, string userId)
		{
			// Arrange
			var mockedService = new Mock<IGiftService>();
			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			var mockedFactory = new Mock<IDtoFactory>();
			var mockedGroupService = new Mock<IGroupService>();
			var mockedInviteService = new Mock<IInviteService>();

			var controller = new GiftController(mockedService.Object, mockedAuthenticationProvider.Object, mockedFactory.Object, mockedGroupService.Object, mockedInviteService.Object);

			// Act
			await controller.DistributeGifts(groupName);

			// Assert
			mockedGroupService.Verify(s => s.GetByName(groupName), Times.Once);
		}

		[TestCase("group", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
		[TestCase("my group", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
		public async Task TestDistributeGifts_GroupIsNull_ShouldReturnNotFound(string groupName, string userId)
		{
			// Arrange
			var mockedService = new Mock<IGiftService>();
			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			var mockedFactory = new Mock<IDtoFactory>();
			var mockedGroupService = new Mock<IGroupService>();
			var mockedInviteService = new Mock<IInviteService>();

			var controller = new GiftController(mockedService.Object, mockedAuthenticationProvider.Object, mockedFactory.Object, mockedGroupService.Object, mockedInviteService.Object);

			// Act
			var result = await controller.DistributeGifts(groupName);

			// Assert
			Assert.IsInstanceOf<NotFoundResult>(result);
		}

		[TestCase("group", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
		[TestCase("my group", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
		public async Task TestDistributeGifts_UserIsNotOwner_ShouldReturnForbidden(string groupName, string userId)
		{
			// Arrange
			var mockedService = new Mock<IGiftService>();

			var user = new User { Id = userId };

			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

			var mockedFactory = new Mock<IDtoFactory>();

			var group = new Group();

			var mockedGroupService = new Mock<IGroupService>();
			mockedGroupService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);

			var mockedInviteService = new Mock<IInviteService>();

			var controller = new GiftController(mockedService.Object, mockedAuthenticationProvider.Object, mockedFactory.Object, mockedGroupService.Object, mockedInviteService.Object);

			// Act
			var result = await controller.DistributeGifts(groupName);

			// Assert
			Assert.IsInstanceOf<ForbidResult>(result);
		}

		[TestCase("group", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
		[TestCase("my group", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
		public async Task TestDistributeGifts_UserIsOwner_ShouldCallServiceDistributeGifts(string groupName, string userId)
		{
			// Arrange
			var mockedService = new Mock<IGiftService>();

			var user = new User { Id = userId };

			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

			var mockedFactory = new Mock<IDtoFactory>();

			var group = new Group { OwnerId = userId };

			var mockedGroupService = new Mock<IGroupService>();
			mockedGroupService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);

			var mockedInviteService = new Mock<IInviteService>();

			var controller = new GiftController(mockedService.Object, mockedAuthenticationProvider.Object, mockedFactory.Object, mockedGroupService.Object, mockedInviteService.Object);

			// Act
			var result = await controller.DistributeGifts(groupName);

			// Assert
			mockedService.Verify(s => s.DistributeGifts(group), Times.Once);
		}

		[TestCase("group", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
		[TestCase("my group", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
		public async Task TestDistributeGifts_UserIsOwner_ShouldCallFactoryCreate(string groupName, string userId)
		{
			// Arrange
			var gifts = new List<Gift> { new Gift(), new Gift(), new Gift() };

			var mockedService = new Mock<IGiftService>();
			mockedService.Setup(s => s.DistributeGifts(It.IsAny<Group>())).ReturnsAsync(gifts);

			var user = new User { Id = userId };

			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

			var mockedFactory = new Mock<IDtoFactory>();

			var group = new Group { OwnerId = userId };

			var mockedGroupService = new Mock<IGroupService>();
			mockedGroupService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);

			var mockedInviteService = new Mock<IInviteService>();

			var controller = new GiftController(mockedService.Object, mockedAuthenticationProvider.Object, mockedFactory.Object, mockedGroupService.Object, mockedInviteService.Object);

			// Act
			var result = await controller.DistributeGifts(groupName);

			// Assert
			mockedFactory.Verify(f => f.CreateGiftListDto(gifts), Times.Once);
		}

		[TestCase("group", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
		[TestCase("my group", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
		public async Task TestDistributeGifts_UserIsOwner_ShouldReturnOkWithCorrectDto(string groupName, string userId)
		{
			// Arrange
			var gifts = new List<Gift> { new Gift(), new Gift(), new Gift() };

			var mockedService = new Mock<IGiftService>();
			mockedService.Setup(s => s.DistributeGifts(It.IsAny<Group>())).ReturnsAsync(gifts);

			var user = new User { Id = userId };

			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

			var dto = new GiftListDto();

			var mockedFactory = new Mock<IDtoFactory>();
			mockedFactory.Setup(f => f.CreateGiftListDto(It.IsAny<IEnumerable<Gift>>())).Returns(dto);

			var group = new Group { OwnerId = userId };

			var mockedGroupService = new Mock<IGroupService>();
			mockedGroupService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);

			var mockedInviteService = new Mock<IInviteService>();

			var controller = new GiftController(mockedService.Object, mockedAuthenticationProvider.Object, mockedFactory.Object, mockedGroupService.Object, mockedInviteService.Object);

			// Act
			var result = await controller.DistributeGifts(groupName);

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			Assert.AreSame(dto, (result as OkObjectResult).Value);
		}
	}
}
