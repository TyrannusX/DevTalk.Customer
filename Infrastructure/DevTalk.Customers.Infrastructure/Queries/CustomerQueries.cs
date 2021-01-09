using Dapper;
using DevTalk.Customers.Domain.Contracts;
using DevTalk.Customers.Domain.Entities.Customer;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTalk.Customers.Infrastructure.Queries
{
    public class CustomerQueries : IQueries<Customer>
    {
        private readonly IConfiguration _configuration;

        public CustomerQueries(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IReadOnlyCollection<Customer>> GetAllAsync()
        {
            var sql = "SELECT * FROM Customers";

            using(var connection = new SqlConnection(_configuration["CustomersConnectionString"]))
            {
                var result = await connection.QueryAsync<Customer>(sql).ConfigureAwait(false);
                return result.ToList();
            }
        }

        public async Task<Customer> GetByIdAsync(Guid id)
        {
            var sql = "SELECT * FROM Customers WHERE CustomerId = @Id";

            using (var connection = new SqlConnection(_configuration["CustomersConnectionString"]))
            {
                return await connection.QuerySingleOrDefaultAsync<Customer>(sql, new { Id = id}).ConfigureAwait(false);
            }
        }
    }
}
