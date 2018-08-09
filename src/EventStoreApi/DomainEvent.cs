using MediatR;
using System;

namespace EventStoreApi
{
    public class DomainEvent: INotification {        
        public Guid CorrelationId { get; set; }
        public Guid CausationId { get; set; }
        public Guid ActivityId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
