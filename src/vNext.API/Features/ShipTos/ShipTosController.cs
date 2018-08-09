using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace vNext.API.Features.ShipTos
{
    [ApiController]
    [Route("api/shipTos")]
    public class ShipTosController
    {
        private ConcurrentDictionary<long, object> dictionary = new ConcurrentDictionary<long, object>();

        private readonly IMediator _mediator;

        public ShipTosController(IMediator mediator)
			=> _mediator = mediator;

        [HttpPost]
        public ActionResult<SaveShipToCommand.Response> Add(SaveShipToCommand.Request request)
        {            
            var key = dictionary.GetOrAdd(request.ShipTo.ShipToId, new object());

            lock (key)
                return _mediator.Send(request).GetAwaiter().GetResult();            
        }
        
        [HttpDelete("{shipToId}/{concurrencyVersion}")]
        public ActionResult<RemoveShipToCommand.Response> Remove(RemoveShipToCommand.Request request)
        {            
            var key = dictionary.GetOrAdd(request.ShipTo.ShipToId, new object());

            lock (key)
                return _mediator.Send(request).GetAwaiter().GetResult();            
        }           

        [HttpGet("{shipToId}")]
        public async Task<ActionResult<GetShipToByIdQuery.Response>> GetById([FromRoute]GetShipToByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetShipTosQuery.Response>> Get()
            => await _mediator.Send(new GetShipTosQuery.Request());
    }
}
