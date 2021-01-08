using DevTalk.Customers.Application.Commands.LoginCustomer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DevTalk.Customers.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly IMediator _mediator;

        public LoginController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Customer([FromBody] LoginCustomerCommand loginCustomerCommand)
        {
            var result = await _mediator.Send(loginCustomerCommand);
            return StatusCode(result ? (int)HttpStatusCode.OK : (int)HttpStatusCode.Unauthorized, result ? "Success" : "UserName or Password is Incorrect.");
        }
    }
}
