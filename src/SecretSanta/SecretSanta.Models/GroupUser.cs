﻿namespace SecretSanta.Models
{
    public class GroupUser
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public int GroupId { get; set; }

        public Group Group { get; set; }
    }
}
