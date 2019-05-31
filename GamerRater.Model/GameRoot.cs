using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace GamerRater.Model
{
    public class GameRoot
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int GameCoverId { get; set; }
        public GameCover GameCover { get; set; }

        [JsonProperty("platforms")]
        [NotMapped]
        public virtual int[] PlatformsIds { get; set; }

        public ICollection<Platform> PlatformList { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
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