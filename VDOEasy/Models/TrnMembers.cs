using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VDOEasy.Models
{
    [Table("trnMembers")]
    public class TrnMembers
    {
        [Key]
        public int ID { get; set; }

        public int BranchID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string IdCardNumber { get; set; }
        public DateTime Birthdate { get; set; }
        public string Address { get; set; }
        public int MemberTypeID { get; set; }
        public DateTime ReceiptDate { get; set; }
        public DateTime IssueDate { get; set; }
        public bool IsActive { get; set; }

        [NotMapped]
        public string BranchName { get; set; }
        [NotMapped]
        public string MemberTypeName { get; set; }
        [NotMapped]
        public int MemberTypePrice { get; set; }
    }
}
