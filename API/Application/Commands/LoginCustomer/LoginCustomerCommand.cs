using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTalk.Customers.Application.Commands.LoginCustomer
{
    public class LoginCustomerCommand : IRequest<bool>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
