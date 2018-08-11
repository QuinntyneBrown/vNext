using MediatR;

namespace vNext.Core.DomainEvents
{
    public class EntitySaved: INotification
    {
        public EntitySaved(string requestType, dynamic id)
        {
            Payload = new
            {
                Id = id,
                RequestType = requestType,                
            };
        }
        public string Type { get; set; } = nameof(EntitySaved);
        public dynamic Payload { get; set; }
    }
}
