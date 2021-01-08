using Common.Application.Events.UserCreated;
using DevTalk.Customers.Domain.Events;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DevTalk.Customers.Application.Events.UserCreated
{
    public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
    {
        private readonly ISendEndpointProvider _sendEndpoint;

        public UserCreatedEventHandler(ISendEndpointProvider publishEndpoint)
        {
            _sendEndpoint = publishEndpoint;
        }

        public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
        {
            var userCreatedIntegrationEven = new UserCreatedIntegrationEvent(notification.Customer.UserName);
            var sender = await _sendEndpoint.GetSendEndpoint(new Uri("sb://reyes-devtalk.servicebus.windows.net/usercreated"));
            await sender.Send(userCreatedIntegrationEven).ConfigureAwait(false);
        }
    }
}
