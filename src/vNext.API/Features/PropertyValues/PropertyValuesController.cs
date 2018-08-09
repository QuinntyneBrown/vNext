using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace vNext.API.Features.PropertyValues
{
    [ApiController]
    [Route("api/propertyValues")]
    public class PropertyValuesController
    {
        private ConcurrentDictionary<long, object> dictionary = new ConcurrentDictionary<long, object>();

        private readonly IMediator _mediator;

        public PropertyValuesController(IMediator mediator)
			=> _mediator = mediator;

        [HttpPost]
        public ActionResult<SavePropertyValueCommand.Response> Add(SavePropertyValueCommand.Request request)
        {            
            var key = dictionary.GetOrAdd(request.PropertyValue.PropertyValueId, new object());

            lock (key)
                return _mediator.Send(request).GetAwaiter().GetResult();            
        }
        
        [HttpDelete("{propertyValueId}/{concurrencyVersion}")]
        public ActionResult<RemovePropertyValueCommand.Response> Remove(RemovePropertyValueCommand.Request request)
        {            
            var key = dictionary.GetOrAdd(request.PropertyValue.PropertyValueId, new object());

            lock (key)
                return _mediator.Send(request).GetAwaiter().GetResult();            
        }           

        [HttpGet("{propertyValueId}")]
        public async Task<ActionResult<GetPropertyValueByIdQuery.Response>> GetById([FromRoute]GetPropertyValueByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetPropertyValuesQuery.Response>> Get()
            => await _mediator.Send(new GetPropertyValuesQuery.Request());
    }
}
