using System.Collections.Generic;
using SecretSanta.Models;
using SecretSanta.Services.Contracts;
using SecretSanta.Data.Contracts;
using System;
using System.Linq;

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
    }
}
