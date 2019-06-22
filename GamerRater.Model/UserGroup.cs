using System.Collections.Generic;

namespace GamerRater.Model
{
    public class UserGroup
    {
        public int Id { get; set; }
        public string Group { get; set; }
        public ICollection<UserHasUserGroup> Users { get; set; }
    }
}