using System;
using System.Collections.Generic;
using System.Text;

namespace GamerRater.Model
{
	public class Platform
	{
		public int Id { get; set; }
		public String Name { get; set; }
		public List<Game> Games { get; } = new List<Game>();
	}
}
