using System;
using System.Collections.Generic;

namespace GamerRater.Model
{
	public class Platform
	{
        [System.ComponentModel.DataAnnotations.KeyAttribute()]
        public int Id { get; set; }
		public String Name { get; set; }
		public List<GameRoot> Games { get; } = new List<GameRoot>();
	}
}
