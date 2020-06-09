using System.Threading.Tasks;
using SecretSanta.Application.Common.Models;

namespace SecretSanta.Application.Common.Interfaces
{
	public interface IUserService
	{
		Task<(Result result, string userId)> CreateUser(string username, string email, string password);
	}
}
