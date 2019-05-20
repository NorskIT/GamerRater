using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GamerRater.Model
{
	public class GameRoot
	{
        public int Id { get; set; }
        public GameCover GameCover { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public int Category { get; set; }
		public int Cover { get; set; }
		public int Created_at { get; set; }
		public double Total_rating { get; set; }
		public string Storyline { get; set; }
		public string Name { get; set; }
		public double Popularity { get; set; }
		public string Summary { get; set; }
	}
}
