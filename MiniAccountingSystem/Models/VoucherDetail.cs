namespace QtecAccountsWeb.Models
{
    // Represents one line item in the VoucherDetails table
    public class VoucherDetail
    {
        public long DetailId { get; set; }
        public long VoucherId { get; set; }
        public int AccountId { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
    }
}