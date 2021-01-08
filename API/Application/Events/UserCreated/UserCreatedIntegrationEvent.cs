using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Application.Events.UserCreated
{
    public class UserCreatedIntegrationEvent
    {
        public string UserName { get; private set; }

        public UserCreatedIntegrationEvent()
        {

        }

        public UserCreatedIntegrationEvent(string userName)
        {
            UserName = userName;
        }
    }
}
