using Dapper;
using Microsoft.Data.SqlClient;
using QtecAccountsWeb.Models;
using System.Data;

namespace QtecAccountsWeb.DataAccess.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IConfiguration _configuration;
        public AccountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Helper method to create a new database connection
        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<IEnumerable<ChartOfAccount>> GetAllAccountsAsync()
        {
            using (var connection = CreateConnection())
            {
                // Here we call our stored procedure using Dapper's QueryAsync method.
                return await connection.QueryAsync<ChartOfAccount>(
                    "sp_ManageChartOfAccounts",
                    new { Action = "GETALL" },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<ChartOfAccount> GetAccountByIdAsync(int id)
        {
            using (var connection = CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<ChartOfAccount>(
                    "sp_ManageChartOfAccounts",
                    new { Action = "GETBYID", AccountId = id },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> CreateAccountAsync(ChartOfAccount account)
        {
            using (var connection = CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("Action", "CREATE");
                parameters.Add("AccountName", account.AccountName);
                parameters.Add("ParentAccountId", account.ParentAccountId);

                // We use ExecuteScalarAsync because our SP returns the ID of the new account.
                return await connection.ExecuteScalarAsync<int>(
                    "sp_ManageChartOfAccounts",
                    parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task UpdateAccountAsync(ChartOfAccount account)
        {
            using (var connection = CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("Action", "UPDATE");
                parameters.Add("AccountId", account.AccountId);
                parameters.Add("AccountName", account.AccountName);
                parameters.Add("ParentAccountId", account.ParentAccountId);

                await connection.ExecuteAsync(
                    "sp_ManageChartOfAccounts",
                    parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task DeleteAccountAsync(int id)
        {
            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync(
                    "sp_ManageChartOfAccounts",
                    new { Action = "DELETE", AccountId = id },
                    commandType: CommandType.StoredProcedure);
            }
        }
    }
}