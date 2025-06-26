using QtecAccountsWeb.Models; // We need our VoucherHeader model

namespace QtecAccountsWeb.DataAccess.Repositories
{
    // This is the contract for our VoucherRepository.
    // It defines all the data operations we can perform for Vouchers.
    public interface IVoucherRepository
    {
        // Method to save a complete voucher (header + details)
        Task<long> SaveVoucherAsync(VoucherHeader voucher);

        // Method to get a list of all voucher headers
        Task<IEnumerable<VoucherHeader>> GetAllVouchersAsync();
    }
}