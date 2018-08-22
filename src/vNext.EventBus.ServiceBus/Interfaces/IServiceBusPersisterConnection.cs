using Microsoft.Azure.ServiceBus;
using System;

namespace vNext.ServiceBus.Interfaces
{
    public interface IServiceBusPersisterConnection
    {
        ServiceBusConnectionStringBuilder ServiceBusConnectionStringBuilder { get; }
        ITopicClient CreateModel();
    }
}
