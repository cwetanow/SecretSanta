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

		public string UserId { get; private set; }
		public string Username { get; private set; }
		public string Email { get; private set; }
		public string DisplayName { get; private set; }
	}
}
