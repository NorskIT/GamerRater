using System;
using System.Collections.Generic;
using System.Text;

namespace GamerRater.Model.IGDBModels
{
	public class GameRoot
	{
		public GameCover GameCover { get; set; }
		public int id { get; set; }
		public int category { get; set; }
		public int cover { get; set; }
		public int created_at { get; set; }
		public List<int> external_games { get; set; }
		public List<int> keywords { get; set; }
		public string name { get; set; }
		public double popularity { get; set; }
		public List<int> similar_games { get; set; }
		public string slug { get; set; }
		public string summary { get; set; }
		public List<int> tags { get; set; }
		public List<int> themes { get; set; }
		public int updated_at { get; set; }
		public string url { get; set; }
	}
}
