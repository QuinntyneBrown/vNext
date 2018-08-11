using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core;
using vNext.Core.DomainEvents;

namespace vNext.API.Features
{
    public class EntitySavedHandler : INotificationHandler<EntitySaved>
    {
        private readonly IHubContext<IntegrationEventsHub> _hubContext;
        public EntitySavedHandler(IHubContext<IntegrationEventsHub> hubContext)
            => _hubContext = hubContext;

        public async Task Handle(EntitySaved notification, CancellationToken cancellationToken)
            => await _hubContext.Clients.All.SendAsync("messages", notification);
    }
}
