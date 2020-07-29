using System.Collections.Generic;
using SecretSanta.Application.Common.Mappings;

namespace SecretSanta.Application.Users.Responses
{
	public class UserListResponse : IMapFrom<IEnumerable<UserProfileResponse>>
	{
		public IEnumerable<UserProfileResponse> Users { get; set; }
	}
}
