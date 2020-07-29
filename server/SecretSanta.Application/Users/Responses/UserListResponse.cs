using System.Collections.Generic;

namespace SecretSanta.Application.Users.Responses
{
	public class UserListResponse
	{
		public IEnumerable<UserProfileResponse> Users { get; set; }
	}
}
