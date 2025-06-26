namespace QtecAccountsWeb.Models
{
    // Represents the VoucherHeader table
    public class VoucherHeader
    {
        public long VoucherId { get; set; }
        public DateTime VoucherDate { get; set; } = DateTime.Today; // Set a default value
        public string VoucherType { get; set; }
        public string? RefNo { get; set; }
        public string? Narration { get; set; }

        // This is the collection of all debit/credit lines for this voucher.
        // It's the "One-to-Many" relationship. One Header has Many Details.
        public List<VoucherDetail> Details { get; set; } = new List<VoucherDetail>();
    }
}