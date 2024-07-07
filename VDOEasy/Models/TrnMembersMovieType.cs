using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VDOEasy.Models
{
    public class TrnMembersMovieType
    {
        [Key]
        public int MemberID { get; set; }
        public int MovieTypeID { get; set; }

        [ForeignKey("MovieTypeID")]
        public MasMovieType MovieType { get; set; }

        [ForeignKey("MemberID")]
        public TrnMembers Member { get; set; }

        [NotMapped]
        public string MovieTypeName { get; set; }
    }
}
