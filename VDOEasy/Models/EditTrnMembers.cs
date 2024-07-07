public class EditTrnMembers
{
    public int ID { get; set; }
    public int BranchID { get; set; }
    public string BranchName { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public DateTime Birthdate { get; set; }
    public string Address { get; set; }
    public string IdCardNumber { get; set; }
    public int MemberTypeID { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime ReceiptDate { get; set; }
    public bool IsActive { get; set; }
    public List<string> MovieTypes { get; set; } 
}
