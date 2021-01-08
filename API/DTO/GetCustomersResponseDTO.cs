using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTalk.Customers.DTO
{
    public class GetCustomersResponseDTO
    {
        public List<GetCustomerResponseDTO> Customers { get; set; }

        public GetCustomersResponseDTO(IReadOnlyCollection<Domain.Entities.Customer.Customer> customers)
        {
            Customers = new List<GetCustomerResponseDTO>();
            customers.ToList().ForEach(x =>
            {
                Customers.Add(new GetCustomerResponseDTO(x));
            });
        }
    }
}
