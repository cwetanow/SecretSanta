using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SecretSanta.Application.Common.Interfaces;
using SecretSanta.Application.Common.Models;
using SecretSanta.Identity.Extensions;

namespace SecretSanta.Identity
{
	public class IdentityService : IIdentityService
	{
		private readonly UserManager<ApplicationUser> userManager;

		public IdentityService(UserManager<ApplicationUser> userManager)
		{
			this.userManager = userManager;
		}

		public async Task<(Result result, string userId)> CreateUser(string username, string email, string password)
		{
			var user = new ApplicationUser(username, email);

			var result = await userManager.CreateAsync(user, password);

			return (result.ToApplicationResult(), user.Id);
		}

		public async Task<Result> AuthenticateUser(string username, string password)
		{
			var user = await userManager.FindByNameAsync(username);

			if (user is null)
			{
				return Result.CreateFailure();
			}

			var success = await userManager.CheckPasswordAsync(user, password);

			return success ? Result.CreateSuccess() : Result.CreateFailure();
		}
	}
}
