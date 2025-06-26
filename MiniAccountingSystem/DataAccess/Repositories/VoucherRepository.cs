using Dapper;
using Microsoft.Data.SqlClient;
using QtecAccountsWeb.Models;
using System.Data; // Required for DataTable

namespace QtecAccountsWeb.DataAccess.Repositories
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly IConfiguration _configuration;

        public VoucherRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<IEnumerable<VoucherHeader>> GetAllVouchersAsync()
        {
            using (var connection = CreateConnection())
            {
                // We will create a simple stored procedure for this.
                // It's cleaner than writing SQL here.
                return await connection.QueryAsync<VoucherHeader>(
                    "sp_GetAllVouchers",
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<long> SaveVoucherAsync(VoucherHeader voucher)
        {
            using (var connection = CreateConnection())
            {
                // ======================================================================
                // DATA TRANSFORMATION STEP
                // We need to convert our C# List<VoucherDetail> into a DataTable
                // so that Dapper can pass it as a Table-Valued Parameter to our SP.
                // ======================================================================
                var detailsTable = new DataTable();
                detailsTable.Columns.Add("AccountId", typeof(int));
                detailsTable.Columns.Add("DebitAmount", typeof(decimal));
                detailsTable.Columns.Add("CreditAmount", typeof(decimal));

                foreach (var detail in voucher.Details)
                {
                    detailsTable.Rows.Add(detail.AccountId, detail.DebitAmount, detail.CreditAmount);
                }

                var parameters = new DynamicParameters();
                // Add all the header information
                parameters.Add("VoucherDate", voucher.VoucherDate);
                parameters.Add("VoucherType", voucher.VoucherType);
                parameters.Add("RefNo", voucher.RefNo);
                parameters.Add("Narration", voucher.Narration);
                parameters.Add("CreatedBy", "1"); // Placeholder for User ID

                // Add our transformed DataTable as a special parameter
                parameters.Add("VoucherDetails", detailsTable.AsTableValuedParameter("dbo.VoucherDetailType"));

                // Call the stored procedure and get the new VoucherId back
                return await connection.ExecuteScalarAsync<long>(
                    "sp_SaveVoucher",
                    parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }
    }
}