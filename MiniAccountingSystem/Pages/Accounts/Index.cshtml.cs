using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QtecAccountsWeb.DataAccess.Repositories; // Your repository
using QtecAccountsWeb.Models; // Your model

namespace QtecAccountsWeb.Pages.Accounts
{
    public class IndexModel : PageModel
    {
        // 1. Create a private variable to hold our repository
        private readonly IAccountRepository _accountRepository;

        // 2. Create a public property to hold the list of accounts for the page to display
        public IEnumerable<ChartOfAccount> Accounts { get; set; }

        // 3. Use the constructor to get the repository object (Dependency Injection)
        public IndexModel(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        // 4. This method runs automatically when the page is loaded (HTTP GET request)
        public async Task OnGetAsync()
        {
            // We call our repository to get all the accounts from the database
            Accounts = await _accountRepository.GetAllAccountsAsync();
        }
    }
}