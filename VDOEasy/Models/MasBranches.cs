using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VDOEasy.Models
{
    [Table("masBranches")]
    public class MasBranches
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
