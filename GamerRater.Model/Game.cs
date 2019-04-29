using System;
using System.Collections.Generic;
using GamerRater.Model.IGDBModels;

namespace GamerRater.Model
{
	public class Game
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public Platform Platform { get; set; }
		public string Desc { get; set; }
		public string ImgFrontUrl { get; set; }
		public string ImgBackUrl { get; set; }
		public List<Rating> Ratings { get; } = new List<Rating>();
	}
}
