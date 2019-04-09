using System;
using System.Collections.Generic;
using System.Text;

namespace GamerRater.Model.IGDBModels
{
	public class GameCover
	{
		public int id { get; set; }
		public int game { get; set; }
		public int height { get; set; }
		public string image_id { get; set; }
		public int width { get; set; }
		public string url { get ; set; }
	}
}
