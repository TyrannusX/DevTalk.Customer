using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTalk.Customers.DTO
{
    public class GetCustomerResponseDTO
    {
        public Guid CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string UserName { get; set; }

        public GetCustomerResponseDTO(Domain.Entities.Customer.Customer customer)
        {
            CustomerId = customer.CustomerId;
            FirstName = customer.FirstName;
            LastName = customer.LastName;
            Address = customer.Address;
            DateOfBirth = customer.DateOfBirth;
            UserName = customer.UserName;
        }
    }
}
