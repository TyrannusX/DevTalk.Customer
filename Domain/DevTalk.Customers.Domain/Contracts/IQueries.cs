using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DevTalk.Customers.Domain.Contracts
{
    public interface IQueries<T> where T : class
    {
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
    }
}
