using MediatR;

namespace vNext.Core.DomainEvents
{
    public class EntitySaved: INotification
    {
        public EntitySaved(string domain, int id)
        {
            Payload = new
            {
                Id = id,
                Domain = "Region"
            };
        }
        public string Type { get; set; } = nameof(EntitySaved);
        public dynamic Payload { get; set; }
    }
}
