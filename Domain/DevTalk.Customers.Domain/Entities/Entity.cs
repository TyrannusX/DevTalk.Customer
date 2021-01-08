using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevTalk.Customers.Domain.Entities
{
    public abstract class Entity
    {
        private List<INotification> _domainEvents;

        public void AddDomainEvent(INotification domainEvent)
        {
            if(_domainEvents == null)
            {
                _domainEvents = new List<INotification>();
            }
            _domainEvents.Add(domainEvent);
        }

        public IReadOnlyCollection<INotification> GetDomainEvents()
        {
            return _domainEvents?.AsReadOnly();
        }
    }
}
