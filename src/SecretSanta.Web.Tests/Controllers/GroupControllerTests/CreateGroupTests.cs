using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SecretSanta.Authentication.Contracts;
using SecretSanta.Services.Contracts;
using SecretSanta.Web.Controllers;
using SecretSanta.Web.Infrastructure;
using SecretSanta.Web.Models.Group;
using System.Threading.Tasks;
using SecretSanta.Common;
using SecretSanta.Models;

namespace SecretSanta.Web.Tests.Controllers.GroupControllerTests
{
    [TestFixture]
    public class CreateGroupTests
    {
        [Test]
        public async Task TestCreateGroup_PassEmptyName_ShouldReturnBadRequest()
        {
            // Arrange
            var mockedService = new Mock<IGroupService>();
            var mockedFactory = new Mock<IDtoFactory>();
            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();

            var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object);

            var dto = new CreateGroupDto();

            // Act
            var result = await controller.CreateGroup(dto);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task TestCreateGroup_PassEmptyName_ShouldSetCorrectErrorMessage()
        {
            // Arrange
            var mockedService = new Mock<IGroupService>();
            var mockedFactory = new Mock<IDtoFactory>();
            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();

            var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object);

            var dto = new CreateGroupDto();

            // Act
            var result = await controller.CreateGroup(dto) as BadRequestObjectResult;

            // Assert
            Assert.AreSame(Constants.GroupNameCannotBeNull, result.Value);
        }

        [TestCase("group")]
        [TestCase("new group name")]
        public async Task TestCreateGroup_PassName_ShouldCallAuthenticationProviderGetCurrentUserAsync(string groupName)
        {
            // Arrange
            var mockedService = new Mock<IGroupService>();
            var mockedFactory = new Mock<IDtoFactory>();
            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
            mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(new User());

            var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object);

            var dto = new CreateGroupDto { GroupName = groupName };

            // Act
            var result = await controller.CreateGroup(dto);

            // Assert
            mockedAuthenticationProvider.Verify(p => p.GetCurrentUserAsync(), Times.Once);
        }

        [TestCase("group", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("new group name", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestCreateGroup_PassName_ShouldCallServiceCreateGroupAsync(string groupName, string userId)
        {
            // Arrange
            var mockedService = new Mock<IGroupService>();
            var mockedFactory = new Mock<IDtoFactory>();
            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
            mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(new User { Id = userId });

            var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object);

            var dto = new CreateGroupDto { GroupName = groupName };

            // Act
            var result = await controller.CreateGroup(dto);

            // Assert
            mockedService.Verify(s => s.CreateGroupAsync(groupName, userId), Times.Once);
        }

        [TestCase("group", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("new group name", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestCreateGroup_ServiceReturnsNull_ShouldReturnBadRequest(string groupName, string userId)
        {
            // Arrange
            var mockedService = new Mock<IGroupService>();
            var mockedFactory = new Mock<IDtoFactory>();
            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
            mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(new User { Id = userId });

            var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object);

            var dto = new CreateGroupDto { GroupName = groupName };

            // Act
            var result = await controller.CreateGroup(dto);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [TestCase("group", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("new group name", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestCreateGroup_ServiceReturnsNull_ShouldSetCorrectErrorMessage(string groupName, string userId)
        {
            // Arrange
            var mockedService = new Mock<IGroupService>();
            var mockedFactory = new Mock<IDtoFactory>();
            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
            mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(new User { Id = userId });

            var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object);

            var dto = new CreateGroupDto { GroupName = groupName };

            // Act
            var result = await controller.CreateGroup(dto) as BadRequestObjectResult;

            // Assert
            Assert.AreSame(Constants.GroupAlreadyExists, result.Value);
        }

        [TestCase("group", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("new group name", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestCreateGroup_ServiceReturnsGroup_ShouldCallFactoryCreate(string groupName, string userId)
        {
            // Arrange
            var group = new Group();

            var mockedService = new Mock<IGroupService>();
            mockedService.Setup(s => s.CreateGroupAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(group);

            var mockedFactory = new Mock<IDtoFactory>();

            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
            mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(new User { Id = userId });

            var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object);

            var dto = new CreateGroupDto { GroupName = groupName };

            // Act
            var result = await controller.CreateGroup(dto);

            // Assert
            mockedFactory.Verify(f => f.CreateGroupDto(group), Times.Once);
        }

        [TestCase("group", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("new group name", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestCreateGroup_ServiceReturnsGroup_ShouldReturnOk(string groupName, string userId)
        {
            // Arrange
            var group = new Group();

            var mockedService = new Mock<IGroupService>();
            mockedService.Setup(s => s.CreateGroupAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(group);

            var mockedFactory = new Mock<IDtoFactory>();

            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
            mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(new User { Id = userId });

            var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object);

            var dto = new CreateGroupDto { GroupName = groupName };

            // Act
            var result = await controller.CreateGroup(dto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [TestCase("group", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("new group name", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestCreateGroup_ServiceReturnsGroup_ShouldSetBodyCorrectly(string groupName, string userId)
        {
            // Arrange
            var group = new Group();

            var mockedService = new Mock<IGroupService>();
            mockedService.Setup(s => s.CreateGroupAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(group);

            var expectedDto = new GroupDto();

            var mockedFactory = new Mock<IDtoFactory>();
            mockedFactory.Setup(f => f.CreateGroupDto(It.IsAny<Group>())).Returns(expectedDto);

            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
            mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(new User { Id = userId });

            var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object);

            var dto = new CreateGroupDto { GroupName = groupName };

            // Act
            var result = await controller.CreateGroup(dto) as OkObjectResult;

            // Assert
            Assert.AreSame(expectedDto, result.Value);
        }
    }
}
