using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevTalk.Customers.Infrastructure.EntityConfigurations.Customer
{
    public class CustomerEntityConfiguration : IEntityTypeConfiguration<DevTalk.Customers.Domain.Entities.Customer.Customer>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<DevTalk.Customers.Domain.Entities.Customer.Customer> builder)
        {
            builder.HasKey(x => x.CustomerId);
        }
    }
}