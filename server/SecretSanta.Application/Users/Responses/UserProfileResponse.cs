using SecretSanta.Application.Common.Mappings;
using SecretSanta.Domain.Entities;

namespace SecretSanta.Application.Users.Responses
{
	public class UserProfileResponse : IMapFrom<User>
	{
		public int Id { get; set; }
		public string DisplayName { get; set; }
		public string Email { get; set; }
		public string Username { get; set; }
	}
}
