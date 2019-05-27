using System;

namespace GamerRater.Model
{
	public class Review
	{
		public int Id { get; set; }
		public DateTime date { get; set; }
		public int GameRootId { get; set; }
		public int UserId { get; set; }
		public virtual User User { get; set; }
		public int Stars { get; set; }
		public string ReviewText { get; set; }
	}
}
