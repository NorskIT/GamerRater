using System.Collections.Generic;

namespace GamerRater.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<UserHasUserGroup> UserGroups { get; set; }
    }
}