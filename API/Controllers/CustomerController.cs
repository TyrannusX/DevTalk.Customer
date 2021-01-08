using DevTalk.Customers.Application.Commands.CreateCustomer;
using DevTalk.Customers.Domain.Contracts;
using DevTalk.Customers.Domain.Entities.Customer;
using DevTalk.Customers.DTO;
using DevTalk.Customers.Exceptions;
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
    public class CustomerController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IQueries<Customer> _queries;

        public CustomerController(IMediator mediator, IQueries<Customer> customer)
        {
            _mediator = mediator;
            _queries = customer;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Customers()
        {
            var result = await _queries.GetAllAsync();
            return Ok(new GetCustomersResponseDTO(result));
        }

        [HttpGet]
        [Route("{customerId}")]
        [Authorize]
        public async Task<IActionResult> Customers([FromRoute] Guid customerId)
        {
            var result = await _queries.GetByIdAsync(customerId);
            if(result == null)
            {
                throw new NotFoundException($"Customer with ID {customerId} not found");
            }
            return Ok(new GetCustomerResponseDTO(result));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Customer([FromBody] CreateCustomerCommand createCustomerCommand)
        {
            await _mediator.Send(createCustomerCommand);
            return StatusCode((int)HttpStatusCode.Created);
        }
    }
}
