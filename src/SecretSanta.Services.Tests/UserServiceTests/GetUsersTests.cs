using Moq;
using NUnit.Framework;
using SecretSanta.Data.Contracts;
using SecretSanta.Models;
using System.Collections.Generic;
using System.Linq;

namespace SecretSanta.Services.Tests.UserServiceTests
{
    [TestFixture]
    public class GetUsersTests
    {
        [TestCase(5, 0, true, null)]
        public void TestGetUsers_ShouldCallRepositoryGetAll(int offset, int limit, bool sortAscending, string searchPattern)
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<User>>();

            var service = new UserService(mockedRepository.Object);

            // Act
            service.GetUsers(offset, limit, sortAscending, searchPattern);

            // Assert
            mockedRepository.Verify(r => r.All, Times.Once);
        }

        [TestCase(0, 5, true, null)]
        public void TestGetUsers_NoSearchPattern_ShouldReturnWholeCollection(int offset, int limit, bool sortAscending, string searchPattern)
        {
            // Arrange
            var users = new List<User> { new User() }
                .AsQueryable();

            var mockedRepository = new Mock<IRepository<User>>();
            mockedRepository.Setup(r => r.All).Returns(users);

            var mockedUnitOfWork = new Mock<IUnitOfWork>();

            var service = new UserService(mockedRepository.Object);

            // Act
            var result = service.GetUsers(offset, limit, sortAscending, searchPattern);

            // Assert
            CollectionAssert.AreEqual(users, result);
        }

        [TestCase(0, 5, true, "name")]
        public void TestGetUsers_WithSearchPattern_ShouldFilterCorrectly(int offset, int limit, bool sortAscending, string searchPattern)
        {
            // Arrange
            var users = new List<User> {
                new User { DisplayName = "test name" },
                new User { DisplayName = string.Empty, UserName="test name" }
            }
                .AsQueryable();

            var mockedRepository = new Mock<IRepository<User>>();
            mockedRepository.Setup(r => r.All).Returns(users);

            var mockedUnitOfWork = new Mock<IUnitOfWork>();

            var service = new UserService(mockedRepository.Object);

            var expected = users.Where(u => u.DisplayName.Contains(searchPattern) || u.UserName.Contains(searchPattern))
                .OrderBy(u => u.DisplayName);

            // Act
            var result = service.GetUsers(offset, limit, sortAscending, searchPattern);

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        [TestCase(0, 5, true, null)]
        public void TestGetUsers_OrderByAscending_ShouldOrderCorrectly(int offset, int limit, bool sortAscending, string searchPattern)
        {
            // Arrange
            var users = new List<User> {
                new User { DisplayName = "test name" },
                new User { DisplayName = string.Empty, UserName="test name" }
            }
                .AsQueryable();

            var mockedRepository = new Mock<IRepository<User>>();
            mockedRepository.Setup(r => r.All).Returns(users);

            var mockedUnitOfWork = new Mock<IUnitOfWork>();

            var service = new UserService(mockedRepository.Object);

            var expected = users
                .OrderBy(u => u.DisplayName);

            // Act
            var result = service.GetUsers(offset, limit, sortAscending, searchPattern);

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        [TestCase(0, 5, false, null)]
        public void TestGetUsers_OrderByDescending_ShouldOrderCorrectly(int offset, int limit, bool sortAscending, string searchPattern)
        {
            // Arrange
            var users = new List<User> {
                new User { DisplayName = "test name" },
                new User { DisplayName = string.Empty, UserName="test name" }
            }
                .AsQueryable();

            var mockedRepository = new Mock<IRepository<User>>();
            mockedRepository.Setup(r => r.All).Returns(users);

            var mockedUnitOfWork = new Mock<IUnitOfWork>();

            var service = new UserService(mockedRepository.Object);

            var expected = users
                .OrderByDescending(u => u.DisplayName);

            // Act
            var result = service.GetUsers(offset, limit, sortAscending, searchPattern);

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        [TestCase(0, 5, true, null)]
        [TestCase(1, 5, true, null)]
        [TestCase(0, 8, true, null)]
        [TestCase(7, 2, true, null)]
        public void TestGetUsers_ShouldPageCorrectly(int offset, int limit, bool sortAscending, string searchPattern)
        {
            // Arrange
            var users = new List<User> {
                new User { DisplayName = "test name" },
                new User { DisplayName = string.Empty },
                new User { DisplayName = "test name" },
                new User { DisplayName = string.Empty },
                new User { DisplayName = "test name" },
                new User { DisplayName = string.Empty },
                new User { DisplayName = "test name" },
                new User { DisplayName = string.Empty },
                new User { DisplayName = "test name" },
                new User { DisplayName = string.Empty },
                new User { DisplayName = "test name" },
                new User { DisplayName = string.Empty },
            }
                .AsQueryable();

            var mockedRepository = new Mock<IRepository<User>>();
            mockedRepository.Setup(r => r.All).Returns(users);

            var mockedUnitOfWork = new Mock<IUnitOfWork>();

            var service = new UserService(mockedRepository.Object);

            var expected = users
                .OrderBy(u => u.DisplayName)
                .Skip(offset)
                .Take(limit);

            // Act
            var result = service.GetUsers(offset, limit, sortAscending, searchPattern);

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }
    }
}
