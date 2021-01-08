using DevTalk.Customers.Domain.Entities.Customer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DevTalk.Customers.Domain.Contracts
{
    public interface IDbContext
    {
        DbSet<Customer> Customers { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<int> SaveDomainAsync(CancellationToken cancellationToken = default);
    }
}
