namespace QtecAccountsWeb.Models
{    public class ChartOfAccount
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public int? ParentAccountId { get; set; } 
        public string? ParentAccountName { get; set; }
        public bool IsActive { get; set; }
    }
}