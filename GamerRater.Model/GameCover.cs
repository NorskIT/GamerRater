using System.ComponentModel.DataAnnotations.Schema;

namespace GamerRater.Model
{
    public class GameCover
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public int game { get; set; }
        public int height { get; set; }
        public string image_id { get; set; }
        public int width { get; set; }
        public string url { get; set; }
    }
}