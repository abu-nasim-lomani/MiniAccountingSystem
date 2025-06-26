using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QtecAccountsWeb.DataAccess.Repositories;
using QtecAccountsWeb.Models;

namespace QtecAccountsWeb.Pages.Accounts
{
    public class DeleteModel : PageModel
    {
        private readonly IAccountRepository _accountRepo;

        [BindProperty]
        public ChartOfAccount Account { get; set; }

        public DeleteModel(IAccountRepository accountRepo)
        {
            _accountRepo = accountRepo;
        }

        // This OnGet fetches the account details to display on the confirmation page.
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Account = await _accountRepo.GetAccountByIdAsync(id.Value);

            if (Account == null)
            {
                return NotFound();
            }
            return Page();
        }

        // This OnPost runs when the user clicks the final "Delete" button.
        public async Task<IActionResult> OnPostAsync()
        {
            // We call the repository to perform the soft delete (sets IsActive = 0).
            await _accountRepo.DeleteAccountAsync(Account.AccountId);

            return RedirectToPage("./Index");
        }
    }
}