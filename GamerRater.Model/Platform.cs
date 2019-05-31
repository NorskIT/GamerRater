using System.ComponentModel.DataAnnotations.Schema;

namespace GamerRater.Model
{
    public class Platform
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Abbreviation { get; set; }
        public string Alternative_name { get; set; }
        public int Category { get; set; }
        public int Created_at { get; set; }
        public string Name { get; set; }
        public int Platform_logo { get; set; }
        public int Product_family { get; set; }
        public string Slug { get; set; }
        public string Summary { get; set; }
        public int Updated_at { get; set; }
        public string Url { get; set; }
    }
}