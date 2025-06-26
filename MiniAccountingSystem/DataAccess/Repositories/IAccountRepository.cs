using QtecAccountsWeb.Models; 

namespace QtecAccountsWeb.DataAccess.Repositories
{
    // This is the contract for our AccountRepository.
    // It defines all the data operations we can perform for ChartOfAccounts.
    public interface IAccountRepository
    {
        Task<IEnumerable<ChartOfAccount>> GetAllAccountsAsync();
        Task<ChartOfAccount> GetAccountByIdAsync(int id);
        Task<int> CreateAccountAsync(ChartOfAccount account);
        Task UpdateAccountAsync(ChartOfAccount account);
        Task DeleteAccountAsync(int id);
    }
}