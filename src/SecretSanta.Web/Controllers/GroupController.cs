using Microsoft.AspNetCore.Mvc;
using SecretSanta.Authentication.Contracts;
using SecretSanta.Services.Contracts;
using SecretSanta.Web.Infrastructure;
using SecretSanta.Web.Models.Group;
using System;
using System.Threading.Tasks;

namespace SecretSanta.Web.Controllers
{
    public class GroupController : Controller
    {
        private readonly IGroupService groupService;
        private readonly IDtoFactory factory;
        private readonly IAuthenticationProvider authenticationProvider;

        public GroupController(IGroupService groupService, IDtoFactory factory, IAuthenticationProvider authenticationProvider)
        {
            if (groupService == null)
            {
                throw new ArgumentNullException(nameof(groupService));
            }

            if (authenticationProvider == null)
            {
                throw new ArgumentNullException(nameof(authenticationProvider));
            }

            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            this.groupService = groupService;
            this.factory = factory;
            this.authenticationProvider = authenticationProvider;
        }
    }
}
