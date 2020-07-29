using System.Threading.Tasks;
using SecretSanta.Application.Common.Models;

namespace SecretSanta.Application.Common.Interfaces
{
	public interface IIdentityService
	{
		Task<(Result result, string userId)> CreateUser(string username, string email, string password);

		Task<Result> AuthenticateUser(string username, string password);
	}
}
