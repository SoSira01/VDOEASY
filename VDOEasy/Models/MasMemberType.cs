using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VDOEasy.Models
{
    [Table("masMemberType")]
    public class MasMemberType
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
    }

}
