using DevTalk.Customers.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevTalk.Customers.Domain.Entities.Customer
{
    public class Customer : Entity
    {
        public Guid CustomerId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Address { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public string Salt { get; private set; }

        public Customer()
        {

        }

        public Customer(string firstName, string lastName, string address, DateTime dob, string user, string pass, string salt)
        {
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            DateOfBirth = dob;
            UserName = user;
            Password = pass;
            Salt = salt;

            AddDomainEvent(new UserCreatedEvent(this));
        }
    }
}
