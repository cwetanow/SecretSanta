using System.Linq;
using Microsoft.AspNetCore.Identity;
using SecretSanta.Application.Common.Models;

namespace SecretSanta.Identity.Extensions
{
	public static class IdentityResultExtensions
	{
		public static Result ToApplicationResult(this IdentityResult result) => result.Succeeded
			? Result.CreateSuccess()
			: Result.CreateFailure(result.Errors.Select(e => e.Description).ToArray());
	}
}
