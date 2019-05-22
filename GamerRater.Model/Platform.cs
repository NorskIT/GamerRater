using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamerRater.Model
{
	public class Platform
	{
        [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.None)]
        public int Id { get; set; }
		public string Name { get; set; }
		public List<GameRoot> Games { get; } = new List<GameRoot>();
	}
}
