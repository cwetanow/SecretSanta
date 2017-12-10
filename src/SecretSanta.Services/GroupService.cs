﻿using SecretSanta.Data.Contracts;
using SecretSanta.Factories;
using SecretSanta.Models;
using SecretSanta.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SecretSanta.Services
{
    public class GroupService : IGroupService
    {
        private readonly IRepository<Group> repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IGroupFactory groupFactory;

        public GroupService(IRepository<Group> repository, IUnitOfWork unitOfWork, IGroupFactory groupFactory)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if (groupFactory == null)
            {
                throw new ArgumentNullException(nameof(groupFactory));
            }

            if (unitOfWork == null)
            {
                throw new ArgumentNullException(nameof(unitOfWork));
            }

            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.groupFactory = groupFactory;
        }

        public async Task<Group> CreateGroupAsync(string groupName, string ownerId)
        {
            var existingGroup = this.GetByName(groupName);

            if (existingGroup != null)
            {
                return null;
            }

            var group = this.groupFactory.CreateGroup(groupName, ownerId);

            this.repository.Add(group);
            await this.unitOfWork.CommitAsync();

            return group;
        }

        public Group GetByName(string groupName)
        {
            var group = this.repository.All
                .Include(g => g.Owner)
                .Include(g => g.Users)
                .ThenInclude(gu => gu.User)
                .FirstOrDefault(g => g.GroupName.Equals(groupName));

            return group;
        }

        public IEnumerable<User> GetGroupUsers(string groupName)
        {
            var group = this.GetByName(groupName);

            if (group == null)
            {
                return null;
            }

            var users = group.Users
                .Select(gu => gu.User)
                .ToList();

            return users;
        }
    }
}
