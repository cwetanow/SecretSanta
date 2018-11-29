using System.Collections.Generic;
using SecretSanta.Models;
using SecretSanta.Services.Contracts;
using SecretSanta.Data.Contracts;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SecretSanta.Services
{
	public class UserService : IUserService
	{
		private readonly IRepository<User> repository;

		public UserService(IRepository<User> repository)
		{
			if (repository == null)
			{
				throw new ArgumentNullException(nameof(repository));
			}

			this.repository = repository;
		}

		public User GetByUsername(string username)
		{
			var user = this.repository.All
				.FirstOrDefault(u => u.UserName.Equals(username));

			return user;
		}

		public IEnumerable<User> GetUsers(int offset, int limit, bool sortAscending = true, string searchPattern = null)
		{
			var result = this.repository.All;

			if (!string.IsNullOrEmpty(searchPattern))
			{
				result = result
					.Where(u => u.DisplayName.Contains(searchPattern) || u.UserName.Contains(searchPattern));
			}

			if (sortAscending)
			{
				result = result
					.OrderBy(u => u.DisplayName);
			}
			else
			{
				result = result
					.OrderByDescending(u => u.DisplayName);
			}

			result = result
				.Skip(offset)
				.Take(limit);

			return result.ToList();
		}

		public IEnumerable<User> GetUsersNotInGroup(string groupName, string pattern = null)
		{
			var query = this.repository
				.All
				.Include(u => u.Groups)
				.Include(u => u.JoinedGroups)
				.ThenInclude(g => g.Group)
				.Include(u => u.Invites)
				.ThenInclude(i => i.Group)
				.Where(u => !u.Invites.Any(i => i.Group.GroupName.Equals(groupName))
				&& !u.JoinedGroups.Any(g => g.Group.GroupName.Equals(groupName))
				&& !u.Groups.Any(g => g.GroupName.Equals(groupName)));

			if (!string.IsNullOrEmpty(pattern))
			{
				query = query
						.Where(u => u.UserName.Contains(pattern) || u.DisplayName.Contains(pattern));
			}

			var users = query.ToList();

			return users;
		}
	}
}
