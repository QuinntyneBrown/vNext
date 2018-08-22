using System.Threading.Tasks;

namespace vNext.ServiceBus.Interfaces
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
