using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SecretSanta.Authentication.Contracts;
using SecretSanta.Models;
using SecretSanta.Services.Contracts;
using SecretSanta.Web.Controllers;
using SecretSanta.Web.Infrastructure;
using SecretSanta.Web.Models.Gift;
using System.Threading.Tasks;

namespace SecretSanta.Web.Tests.Controllers.GiftControllerTests
{
	[TestFixture]
	public class GetGroupGiftTests
	{
		[TestCase(1, "group", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
		[TestCase(5, "my group", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
		public async Task TestGetGroupGift_ShouldCallAuthenticationProviderGetCurrentUserAsync(int groupId, string groupName, string userId)
		{
			// Arrange
			var mockedService = new Mock<IGiftService>();
			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			var mockedFactory = new Mock<IDtoFactory>();
			var mockedGroupService = new Mock<IGroupService>();
			var mockedInviteService = new Mock<IInviteService>();

			var controller = new GiftController(mockedService.Object, mockedAuthenticationProvider.Object, mockedFactory.Object, mockedGroupService.Object, mockedInviteService.Object);

			// Act
			await controller.GetGroupGift(groupName);

			// Assert
			mockedAuthenticationProvider.Verify(p => p.GetCurrentUserAsync(), Times.Once);
		}

		[TestCase(1, "group", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
		[TestCase(5, "my group", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
		public async Task TestGetGroupGift_ShouldCallGroupServiceGetByName(int groupId, string groupName, string userId)
		{
			// Arrange
			var mockedService = new Mock<IGiftService>();
			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			var mockedFactory = new Mock<IDtoFactory>();
			var mockedGroupService = new Mock<IGroupService>();
			var mockedInviteService = new Mock<IInviteService>();

			var controller = new GiftController(mockedService.Object, mockedAuthenticationProvider.Object, mockedFactory.Object, mockedGroupService.Object, mockedInviteService.Object);

			// Act
			await controller.GetGroupGift(groupName);

			// Assert
			mockedGroupService.Verify(s => s.GetByName(groupName), Times.Once);
		}

		[TestCase(1, "group", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
		[TestCase(5, "my group", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
		public async Task TestGetGroupGift_GroupIsNull_ShouldReturnNotFound(int groupId, string groupName, string userId)
		{
			// Arrange
			var mockedService = new Mock<IGiftService>();
			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			var mockedFactory = new Mock<IDtoFactory>();
			var mockedGroupService = new Mock<IGroupService>();
			var mockedInviteService = new Mock<IInviteService>();

			var controller = new GiftController(mockedService.Object, mockedAuthenticationProvider.Object, mockedFactory.Object, mockedGroupService.Object, mockedInviteService.Object);

			// Act
			var result = await controller.GetGroupGift(groupName);

			// Assert
			Assert.IsInstanceOf<NotFoundResult>(result);
		}

		[TestCase(1, "group", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
		[TestCase(5, "my group", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
		public async Task TestGetGroupGift_UserIsInvited_ShouldCallServiceGetGiftInGroup(int groupId, string groupName, string userId)
		{
			// Arrange
			var mockedService = new Mock<IGiftService>();

			var user = new User { Id = userId };

			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

			var mockedFactory = new Mock<IDtoFactory>();

			var group = new Group { Id = groupId };

			var mockedGroupService = new Mock<IGroupService>();
			mockedGroupService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);

			var mockedInviteService = new Mock<IInviteService>();
			mockedInviteService.Setup(s => s.IsUserInvited(It.IsAny<int>(), It.IsAny<string>())).Returns(true);

			var controller = new GiftController(mockedService.Object, mockedAuthenticationProvider.Object, mockedFactory.Object, mockedGroupService.Object, mockedInviteService.Object);

			// Act
			var result = await controller.GetGroupGift(groupName);

			// Assert
			mockedService.Verify(s => s.GetGiftInGroup(groupId, userId), Times.Once);
		}

		[TestCase(1, "group", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
		[TestCase(5, "my group", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
		public async Task TestGetGroupGift_UserIsInvited_ShouldCallFactoryCreateGiftDto(int groupId, string groupName, string userId)
		{
			// Arrange
			var gift = new Gift();

			var mockedService = new Mock<IGiftService>();
			mockedService.Setup(s => s.GetGiftInGroup(It.IsAny<int>(), It.IsAny<string>())).Returns(gift);

			var user = new User { Id = userId };

			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

			var mockedFactory = new Mock<IDtoFactory>();

			var group = new Group { Id = groupId };

			var mockedGroupService = new Mock<IGroupService>();
			mockedGroupService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);

			var mockedInviteService = new Mock<IInviteService>();
			mockedInviteService.Setup(s => s.IsUserInvited(It.IsAny<int>(), It.IsAny<string>())).Returns(true);

			var controller = new GiftController(mockedService.Object, mockedAuthenticationProvider.Object, mockedFactory.Object, mockedGroupService.Object, mockedInviteService.Object);

			// Act
			var result = await controller.GetGroupGift(groupName);

			// Assert
			mockedFactory.Verify(f => f.CreateGiftDto(gift), Times.Once);
		}

		[TestCase(1, "group", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
		[TestCase(5, "my group", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
		public async Task TestGetGroupGift_GiftIsNull_ShouldCallFactoryCreateHasGiftDtoCorrectly(int groupId, string groupName, string userId)
		{
			// Arrange
			var gift = new Gift();

			var mockedService = new Mock<IGiftService>();

			var user = new User { Id = userId };

			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

			var dto = new GiftDto();

			var mockedFactory = new Mock<IDtoFactory>();
			mockedFactory.Setup(f => f.CreateGiftDto(It.IsAny<Gift>())).Returns(dto);

			var group = new Group { Id = groupId };

			var mockedGroupService = new Mock<IGroupService>();
			mockedGroupService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);

			var mockedInviteService = new Mock<IInviteService>();
			mockedInviteService.Setup(s => s.IsUserInvited(It.IsAny<int>(), It.IsAny<string>())).Returns(true);

			var controller = new GiftController(mockedService.Object, mockedAuthenticationProvider.Object, mockedFactory.Object, mockedGroupService.Object, mockedInviteService.Object);

			// Act
			var result = await controller.GetGroupGift(groupName);

			// Assert
			mockedFactory.Verify(f => f.CreateHasGiftDto(false, dto), Times.Once);
		}

		[TestCase(1, "group", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
		[TestCase(5, "my group", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
		public async Task TestGetGroupGift_GiftIsNotNull_ShouldCallFactoryCreateHasGiftDtoCorrectly(int groupId, string groupName, string userId)
		{
			// Arrange
			var gift = new Gift();

			var mockedService = new Mock<IGiftService>();
			mockedService.Setup(s => s.GetGiftInGroup(It.IsAny<int>(), It.IsAny<string>())).Returns(gift);

			var user = new User { Id = userId };

			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

			var dto = new GiftDto();

			var mockedFactory = new Mock<IDtoFactory>();
			mockedFactory.Setup(f => f.CreateGiftDto(It.IsAny<Gift>())).Returns(dto);

			var group = new Group { Id = groupId };

			var mockedGroupService = new Mock<IGroupService>();
			mockedGroupService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);

			var mockedInviteService = new Mock<IInviteService>();
			mockedInviteService.Setup(s => s.IsUserInvited(It.IsAny<int>(), It.IsAny<string>())).Returns(true);

			var controller = new GiftController(mockedService.Object, mockedAuthenticationProvider.Object, mockedFactory.Object, mockedGroupService.Object, mockedInviteService.Object);

			// Act
			var result = await controller.GetGroupGift(groupName);

			// Assert
			mockedFactory.Verify(f => f.CreateHasGiftDto(true, dto), Times.Once);
		}

		[TestCase(1, "group", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
		[TestCase(5, "my group", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
		public async Task TestGetGroupGift_ShouldReturnCorrectly(int groupId, string groupName, string userId)
		{
			// Arrange
			var gift = new Gift();

			var mockedService = new Mock<IGiftService>();
			mockedService.Setup(s => s.GetGiftInGroup(It.IsAny<int>(), It.IsAny<string>())).Returns(gift);

			var user = new User { Id = userId };

			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

			var dto = new HasGiftDto();

			var mockedFactory = new Mock<IDtoFactory>();
			mockedFactory.Setup(f => f.CreateHasGiftDto(It.IsAny<bool>(), It.IsAny<GiftDto>())).Returns(dto);

			var group = new Group { Id = groupId };

			var mockedGroupService = new Mock<IGroupService>();
			mockedGroupService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);

			var mockedInviteService = new Mock<IInviteService>();
			mockedInviteService.Setup(s => s.IsUserInvited(It.IsAny<int>(), It.IsAny<string>())).Returns(true);

			var controller = new GiftController(mockedService.Object, mockedAuthenticationProvider.Object, mockedFactory.Object, mockedGroupService.Object, mockedInviteService.Object);

			// Act
			var result = await controller.GetGroupGift(groupName);

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			Assert.AreSame(dto, (result as OkObjectResult).Value);
		}
	}
}
