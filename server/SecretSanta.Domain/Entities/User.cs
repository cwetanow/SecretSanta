using SecretSanta.Domain.Common;

namespace SecretSanta.Domain.Entities
{
	public class User : Entity
	{
		private User() { }

		public User(string userId, string username, string email, string displayName)
		{
			UserId = userId;
			Username = username;
			Email = email;
			DisplayName = displayName;
		}

		public string UserId { get; }
		public string Username { get; }
		public string Email { get; }
		public string DisplayName { get; }
	}
}
