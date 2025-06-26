using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QtecAccountsWeb.DataAccess.Repositories;
using QtecAccountsWeb.Models;

namespace QtecAccountsWeb.Pages.Vouchers
{
    public class IndexModel : PageModel
    {
        private readonly IVoucherRepository _voucherRepo;

        // This property will hold our list of vouchers to display on the page.
        public IEnumerable<VoucherHeader> Vouchers { get; set; }

        public IndexModel(IVoucherRepository voucherRepo)
        {
            _voucherRepo = voucherRepo;
        }

        // This runs automatically when the page is loaded.
        public async Task OnGetAsync()
        {
            // We call our repository to get all the voucher headers from the database.
            Vouchers = await _voucherRepo.GetAllVouchersAsync();
        }
    }
}