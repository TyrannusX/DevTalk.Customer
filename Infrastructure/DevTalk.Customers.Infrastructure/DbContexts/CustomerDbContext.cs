using DevTalk.Customers.Domain.Contracts;
using DevTalk.Customers.Domain.Entities;
using DevTalk.Customers.Domain.Entities.Customer;
using DevTalk.Customers.Infrastructure.EntityConfigurations.Customer;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DevTalk.Customers.Infrastructure.DbContexts
{
    public class CustomerDbContext : DbContext, IDbContext
    {
        private readonly IMediator _mediator;

        public DbSet<Customer> Customers { get; set; }

        public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options){}
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CustomerEntityConfiguration());
        }

        public async Task<int> SaveDomainAsync(CancellationToken cancellationToken = default)
        {
            //Publish customer domain events and then commit
            var domainEntities = ChangeTracker.Entries<Entity>().Where(x => x.Entity.GetDomainEvents() != null).ToList();

            foreach(var entry in domainEntities)
            {
                foreach(var domainEvent in entry.Entity.GetDomainEvents())
                {
                    await _mediator.Publish(domainEvent);
                }
            }

            return await SaveChangesAsync();
        }
    }
}
