using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SecretSanta.Data.Contracts;
using SecretSanta.Factories;
using SecretSanta.Models;

namespace SecretSanta.Services.Tests.GroupServiceTests
{
	[TestFixture]
	public class CloseGroupTests
	{
		[TestCase("testgroup")]
		public async Task TestCloseGroup_ShouldCallRepositoryAll(string groupName)
		{
			// Arrange
			var group = new Group { GroupName = groupName };

			var groups = new List<Group> { group }
				.AsQueryable();

			var mockedRepository = new Mock<IRepository<Group>>();
			mockedRepository.Setup(r => r.All)
				.Returns(groups);

			var mockedUnitOfWork = new Mock<IUnitOfWork>();
			var mockedFactory = new Mock<IGroupFactory>();
			var mockedGroupUserRepository = new Mock<IRepository<GroupUser>>();

			var service = new GroupService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object,
				mockedGroupUserRepository.Object);

			// Act
			await service.CloseGroup(groupName);

			// Assert
			mockedRepository.Verify(r => r.All, Times.Once);
		}

		[TestCase("testgroup")]
		public async Task TestCloseGroup_ShouldSetGroupIsClosedToTrue(string groupName)
		{
			// Arrange
			var group = new Group { GroupName = groupName };

			var groups = new List<Group> { group }
				.AsQueryable();

			var mockedRepository = new Mock<IRepository<Group>>();
			mockedRepository.Setup(r => r.All)
				.Returns(groups);

			var mockedUnitOfWork = new Mock<IUnitOfWork>();
			var mockedFactory = new Mock<IGroupFactory>();
			var mockedGroupUserRepository = new Mock<IRepository<GroupUser>>();

			var service = new GroupService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object,
				mockedGroupUserRepository.Object);

			// Act
			await service.CloseGroup(groupName);

			// Assert
			Assert.IsTrue(group.IsClosed);
		}

		[TestCase("testgroup")]
		public async Task TestCloseGroup_ShouldCallRepositoryUpdate(string groupName)
		{
			// Arrange
			var group = new Group { GroupName = groupName };

			var groups = new List<Group> { group }
				.AsQueryable();

			var mockedRepository = new Mock<IRepository<Group>>();
			mockedRepository.Setup(r => r.All)
				.Returns(groups);

			var mockedUnitOfWork = new Mock<IUnitOfWork>();
			var mockedFactory = new Mock<IGroupFactory>();
			var mockedGroupUserRepository = new Mock<IRepository<GroupUser>>();

			var service = new GroupService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object,
				mockedGroupUserRepository.Object);

			// Act
			await service.CloseGroup(groupName);

			// Assert
			mockedRepository.Verify(r => r.Update(group), Times.Once);
		}

		[TestCase("testgroup")]
		public async Task TestCloseGroup_ShouldCallUnitOfWorkCommitAsync(string groupName)
		{
			// Arrange
			var group = new Group { GroupName = groupName };

			var groups = new List<Group> { group }
				.AsQueryable();

			var mockedRepository = new Mock<IRepository<Group>>();
			mockedRepository.Setup(r => r.All)
				.Returns(groups);

			var mockedUnitOfWork = new Mock<IUnitOfWork>();
			var mockedFactory = new Mock<IGroupFactory>();
			var mockedGroupUserRepository = new Mock<IRepository<GroupUser>>();

			var service = new GroupService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object,
				mockedGroupUserRepository.Object);

			// Act
			await service.CloseGroup(groupName);

			// Assert
			mockedUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
		}
	}
}
