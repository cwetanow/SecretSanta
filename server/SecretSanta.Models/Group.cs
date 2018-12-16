using System.Collections.Generic;

namespace SecretSanta.Models
{
	public class Group
	{
		public Group()
		{
			this.IsClosed = false;
		}

		public Group(string groupName, string ownerId)
		: this()
		{
			this.GroupName = groupName;
			this.OwnerId = ownerId;
		}

		public int Id { get; set; }

		public string GroupName { get; set; }

		public string OwnerId { get; set; }

		public User Owner { get; set; }

		public ICollection<GroupUser> Users { get; set; }

		public ICollection<Invite> Invites { get; set; }

		public bool IsClosed { get; set; }
	}
}
