using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTalk.Customers.Application.Commands.CreateCustomer
{
    public class CreateCustomerCommand : IRequest<bool>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
