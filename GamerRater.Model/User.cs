using System;
using System.Collections.Generic;
using System.Text;

namespace GamerRater.Model
{
	public class User
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
        public int UserGroupId { get; set; }
        public UserGroup UserGroup { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }

    }
}
