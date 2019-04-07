using System;
using System.Collections.Generic;
using System.Text;

namespace GamerRater.Model
{
	public class Rating
	{
		public int Id { get; set; }
		public Game Game { get; set; }
		public Stars Stars { get; set; }
		public string Review { get; set; }
	}
}
