using Microsoft.AspNetCore.Mvc;
using SecretSanta.Services.Contracts;
using SecretSanta.Web.Infrastructure;
using System;

namespace SecretSanta.Web.Controllers
{
    public class GroupController : Controller
    {
        private readonly IGroupService groupService;
        private readonly IDtoFactory factory;

        public GroupController(IGroupService groupService, IDtoFactory factory)
        {
            if (groupService == null)
            {
                throw new ArgumentNullException(nameof(groupService));
            }

            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            this.groupService = groupService;
            this.factory = factory;
        }
    }
}
