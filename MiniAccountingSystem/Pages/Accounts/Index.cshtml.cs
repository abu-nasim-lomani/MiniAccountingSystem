using ClosedXML.Excel; // Required for creating Excel files
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; // Required for IActionResult
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniAccountingSystem.Models;
using QtecAccountsWeb.DataAccess.Repositories;
using QtecAccountsWeb.Models;

namespace QtecAccountsWeb.Pages.Accounts
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IAccountRepository _accountRepository;

        // This property holds the data for the hierarchical tree view.
        public List<AccountNodeViewModel> AccountTree { get; set; } = new List<AccountNodeViewModel>();

        public IndexModel(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task OnGetAsync()
        {
            // Get the flat list of accounts from the database.
            var allAccounts = await _accountRepository.GetAllAccountsAsync();

            // Transform the flat list into a tree structure.
            AccountTree = BuildTree(allAccounts.ToList());
        }

        // This is the recursive function that builds the tree.
        private List<AccountNodeViewModel> BuildTree(List<ChartOfAccount> accounts, int? parentId = null)
        {
            var nodes = new List<AccountNodeViewModel>();
            var childrenOfCurrentParent = accounts.Where(a => a.ParentAccountId == parentId).ToList();

            foreach (var child in childrenOfCurrentParent)
            {
                var node = new AccountNodeViewModel
                {
                    AccountId = child.AccountId,
                    AccountName = child.AccountName,
                    // Recursive call to find the children of this child.
                    Children = BuildTree(accounts, child.AccountId)
                };
                nodes.Add(node);
            }

            return nodes;
        }

        // =============================================================
        //  NEW METHOD for the Bonus Feature: Export to Excel
        // =============================================================
        public async Task<IActionResult> OnPostExportToExcelAsync()
        {
            // 1. Get the dataset from the database.
            var allAccounts = await _accountRepository.GetAllAccountsAsync();

            // 2. Create a new Excel workbook in memory.
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Chart of Accounts");
                var currentRow = 1;

                // 3. Create and style the header row.
                worksheet.Cell(currentRow, 1).Value = "Account ID";
                worksheet.Cell(currentRow, 2).Value = "Account Name";
                worksheet.Cell(currentRow, 3).Value = "Parent Account Name";
                worksheet.Row(currentRow).Style.Font.Bold = true;

                // 4. Loop through the dataset and populate the Excel rows.
                foreach (var account in allAccounts)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = account.AccountId;
                    worksheet.Cell(currentRow, 2).Value = account.AccountName;
                    worksheet.Cell(currentRow, 3).Value = account.ParentAccountName;
                }

                worksheet.Columns().AdjustToContents(); // Auto-fit column widths

                // 5. Save the Excel file to a memory stream and return it for download.
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    var fileName = "ChartOfAccounts.xlsx";

                    return File(content, contentType, fileName);
                }
            }
        }
    }
}
