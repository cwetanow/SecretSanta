namespace SecretSanta.Models
{
    public class GroupUser
    {
        public GroupUser()
        {

        }

        public GroupUser(int groupId, string userId)
        {
            this.GroupId = groupId;
            this.UserId = UserId;
        }

        public int Id { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public int GroupId { get; set; }

        public Group Group { get; set; }
    }
}
