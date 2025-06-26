using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using QtecAccountsWeb.DataAccess.Repositories;
using QtecAccountsWeb.Models;

namespace QtecAccountsWeb.Pages.Accounts
{
    [Authorize(Roles = "Admin,Accountant")]
    public class EditModel : PageModel
    {
        private readonly IAccountRepository _accountRepo;

        [BindProperty]
        public ChartOfAccount Account { get; set; }
        public SelectList ParentAccountList { get; set; }

        public EditModel(IAccountRepository accountRepo)
        {
            _accountRepo = accountRepo;
        }

        // This OnGetAsync takes an 'id' from the URL (e.g., /Accounts/Edit/1)
        // It uses this ID to fetch the specific account we want to edit.
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound(); // If no ID is provided, show a "Not Found" error.
            }

            // Fetch the specific account from the database.
            Account = await _accountRepo.GetAccountByIdAsync(id.Value);

            if (Account == null)
            {
                return NotFound(); // If account with that ID doesn't exist, show error.
            }

            // Populate the dropdown for parent accounts.
            await PopulateParentAccountList();
            return Page();
        }

        // This runs when the "Save" button is clicked.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await PopulateParentAccountList();
                return Page();
            }

            // Call the repository to UPDATE the account in the database.
            await _accountRepo.UpdateAccountAsync(Account);

            // Redirect back to the main list.
            return RedirectToPage("./Index");
        }

        private async Task PopulateParentAccountList()
        {
            var allAccounts = await _accountRepo.GetAllAccountsAsync();
            // We exclude the current account from the parent list to prevent it from being its own parent.
            ParentAccountList = new SelectList(allAccounts.Where(a => a.AccountId != Account.AccountId), "AccountId", "AccountName", Account.ParentAccountId);
        }
    }
}