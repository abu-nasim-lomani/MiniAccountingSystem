using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using QtecAccountsWeb.DataAccess.Repositories;
using QtecAccountsWeb.Models;
using System.Security.Claims; // Required to get the logged-in user's ID

namespace QtecAccountsWeb.Pages.Vouchers
{
    public class CreateModel : PageModel
    {
        private readonly IVoucherRepository _voucherRepo;
        private readonly IAccountRepository _accountRepo;

        // This [BindProperty] will hold all the data from our form.
        // It includes the header info and the list of debit/credit lines.
        [BindProperty]
        public VoucherHeader Voucher { get; set; }

        // This will be used to populate the account dropdowns in the form.
        public SelectList AccountList { get; set; }

        public CreateModel(IVoucherRepository voucherRepo, IAccountRepository accountRepo)
        {
            _voucherRepo = voucherRepo;
            _accountRepo = accountRepo;
        }

        // This runs when the page is first loaded. Its job is to prepare the form.
        public async Task<IActionResult> OnGetAsync()
        {
            // We need to load all accounts to populate the dropdowns on the form.
            await PopulateAccountList();
            // We initialize the Voucher object to avoid errors on the page.
            Voucher = new VoucherHeader();
            return Page();
        }

        // This runs when the user clicks the "Save Voucher" button.
        public async Task<IActionResult> OnPostAsync()
        {
            // === DATA VALIDATION STEP ===
            // 1. Check if the model itself is valid (e.g., required fields are filled).
            if (!ModelState.IsValid)
            {
                await PopulateAccountList();
                return Page();
            }

            // 2. Check the accounting rule: Total Debits must equal Total Credits.
            decimal totalDebit = Voucher.Details.Sum(d => d.DebitAmount);
            decimal totalCredit = Voucher.Details.Sum(d => d.CreditAmount);

            if (totalDebit != totalCredit)
            {
                // If they don't match, add a custom error message and show the form again.
                ModelState.AddModelError(string.Empty, "Total Debit must be equal to Total Credit.");
                await PopulateAccountList();
                return Page();
            }

            // 3. (Optional but good practice) Ensure there's at least one debit and one credit line.
            if (totalDebit == 0)
            {
                ModelState.AddModelError(string.Empty, "Voucher cannot be saved with a zero balance.");
                await PopulateAccountList();
                return Page();
            }

            // === DATA SUBMISSION STEP ===
            // If all validation passes, get the current user's ID and save the voucher.
            // string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Voucher.CreatedBy = userId; // We'll use a placeholder for now.

            await _voucherRepo.SaveVoucherAsync(Voucher);

            // Redirect to a new page that will list all vouchers.
            // We will create this page later.
            return RedirectToPage("./Index");
        }

        // A helper method to get the list of accounts.
        private async Task PopulateAccountList()
        {
            var allAccounts = await _accountRepo.GetAllAccountsAsync();
            AccountList = new SelectList(allAccounts, "AccountId", "AccountName");
        }
    }
}