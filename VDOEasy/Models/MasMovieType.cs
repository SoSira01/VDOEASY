using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VDOEasy.Models
{
    [Table("masMoviesType")]
    public class MasMovieType
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
