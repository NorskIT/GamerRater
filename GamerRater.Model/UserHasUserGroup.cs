using System;
using System.Collections.Generic;
using System.Text;

namespace GamerRater.Model
{
    public class UserHasUserGroup
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int UserGroupId { get; set; }
        public UserGroup UserGroup { get; set; }
    }
}
