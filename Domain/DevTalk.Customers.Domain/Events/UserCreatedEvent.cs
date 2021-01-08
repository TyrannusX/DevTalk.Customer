using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevTalk.Customers.Domain.Events
{
    public class UserCreatedEvent : INotification
    {
        public Customers.Domain.Entities.Customer.Customer Customer { get; private set; }

        public UserCreatedEvent(Customers.Domain.Entities.Customer.Customer customer)
        {
            Customer = customer;
        }
    }
}
