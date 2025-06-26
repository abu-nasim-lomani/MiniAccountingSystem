namespace MiniAccountingSystem.Models
{
    public class AccountNodeViewModel
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public List<AccountNodeViewModel> Children { get; set; } = new List<AccountNodeViewModel>();
    }
}