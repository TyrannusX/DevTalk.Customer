using DevTalk.Customers.Domain.Contracts;
using DevTalk.Customers.Domain.Entities.Customer;
using DevTalk.Customers.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DevTalk.Customers.Application.Commands.CreateCustomer
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, bool>
    {
        private readonly IDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public CreateCustomerCommandHandler(IDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<bool> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = _dbContext.Customers.SingleOrDefault(x => x.UserName == request.UserName);
            if(customer != null)
            {
                throw new BadRequestException($"Customer with username {request.UserName} already exists");
            }

            //generate salt
            var randomBytes = new byte[128 / 8];
            string salt;
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(randomBytes);
                salt = Convert.ToBase64String(randomBytes);
            }

            //hash password
            var securedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: request.Password,
                salt: Encoding.UTF8.GetBytes(salt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            customer = new Customer(
                request.FirstName,
                request.LastName,
                request.Address,
                request.DateOfBirth,
                request.UserName,
                securedPassword,
                salt
                );

            //store customer
            _dbContext.Customers.Add(customer);
            await _dbContext.SaveDomainAsync().ConfigureAwait(false);

            return true;
        }
    }
}
