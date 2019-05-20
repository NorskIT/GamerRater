using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GamerRater.Model
{
	public class GameRoot
	{
        [System.ComponentModel.DataAnnotations.KeyAttribute()]
        public int id { get; set; }
        public GameCover GameCover { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public int Category { get; set; }
		public int Cover { get; set; }
		public int Created_at { get; set; }
        [NotMapped]
		public int[] External_games { get; set; }
		public double Total_rating { get; set; }
        [NotMapped]
        public int[] Keywords { get; set; }
		public string Storyline { get; set; }
		public string Name { get; set; }
		public double Popularity { get; set; }
        [NotMapped]
        public int[] Similar_games { get; set; }
		public string Slug { get; set; }
		public string Summary { get; set; }
        [NotMapped]
        public int[] Tags { get; set; }
        [NotMapped]
        public int[] Themes { get; set; }
		public int Updated_at { get; set; }
		public string Url { get; set; }
        [NotMapped]
        public int[] Platforms { get; set; }
	}
}
