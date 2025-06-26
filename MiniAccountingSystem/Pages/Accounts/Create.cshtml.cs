using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering; // Required for SelectList
using QtecAccountsWeb.DataAccess.Repositories;
using QtecAccountsWeb.Models;

namespace QtecAccountsWeb.Pages.Accounts
{
    public class CreateModel : PageModel
    {
        private readonly IAccountRepository _accountRepo;

        // [BindProperty] tells the application to automatically take the form data
        // and put it into this "Account" object when the form is submitted.
        [BindProperty]
        public ChartOfAccount Account { get; set; }

        // This SelectList will be used to create the <select> dropdown in the HTML form.
        public SelectList ParentAccountList { get; set; }

        public CreateModel(IAccountRepository accountRepo)
        {
            _accountRepo = accountRepo;
        }

        // This runs when the page is first loaded (HTTP GET).
        // Its job is to prepare the form, like populating the dropdown.
        public async Task<IActionResult> OnGetAsync()
        {
            await PopulateParentAccountList();
            return Page();
        }

        // This runs when the "Create" button is clicked (HTTP POST).
        // Its job is to validate the input data and save it.
        public async Task<IActionResult> OnPostAsync()
        {
            // ModelState.IsValid checks for basic validation like if a required field is empty.
            if (!ModelState.IsValid)
            {
                // If the data is not valid, show the form again.
                await PopulateParentAccountList();
                return Page();
            }

            // Call the repository to save the new account to the database.
            await _accountRepo.CreateAccountAsync(Account);

            // After saving, redirect the user back to the main accounts list page.
            return RedirectToPage("./Index");
        }

        // A helper method to avoid repeating code.
        // It gets all accounts and prepares them for the dropdown.
        private async Task PopulateParentAccountList()
        {
            var allAccounts = await _accountRepo.GetAllAccountsAsync();
            ParentAccountList = new SelectList(allAccounts, "AccountId", "AccountName");
        }
    }
}