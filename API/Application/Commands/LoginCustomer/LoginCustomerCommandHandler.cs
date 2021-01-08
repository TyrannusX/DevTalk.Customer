using DevTalk.Customers.Domain.Contracts;
using DevTalk.Customers.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DevTalk.Customers.Application.Commands.LoginCustomer
{
    public class LoginCustomerCommandHandler : IRequestHandler<LoginCustomerCommand, bool>
    {
        private readonly IDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public LoginCustomerCommandHandler(IDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<bool> Handle(LoginCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _dbContext.Customers.SingleOrDefaultAsync(x => x.UserName == request.UserName).ConfigureAwait(false);
            if(customer == null)
            {
                throw new NotFoundException("Customer Not Found");
            }

            var enteredPasswordHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: request.Password,
                salt: Encoding.UTF8.GetBytes(customer.Salt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return enteredPasswordHash == customer.Password;
        }
    }
}
