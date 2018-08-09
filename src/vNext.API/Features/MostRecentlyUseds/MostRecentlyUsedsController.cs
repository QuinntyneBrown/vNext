using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace vNext.API.Features.MostRecentlyUseds
{
    [ApiController]
    [Route("api/mostRecentlyUseds")]
    public class MostRecentlyUsedsController
    {
        private ConcurrentDictionary<long, object> dictionary = new ConcurrentDictionary<long, object>();

        private readonly IMediator _mediator;

        public MostRecentlyUsedsController(IMediator mediator)
			=> _mediator = mediator;

        [HttpPost]
        public ActionResult<SaveMostRecentlyUsedCommand.Response> Add(SaveMostRecentlyUsedCommand.Request request)
        {            
            var key = dictionary.GetOrAdd(request.MostRecentlyUsed.MostRecentlyUsedId, new object());

            lock (key)
                return _mediator.Send(request).GetAwaiter().GetResult();            
        }
        
        [HttpDelete("{mostRecentlyUsedId}/{concurrencyVersion}")]
        public ActionResult<RemoveMostRecentlyUsedCommand.Response> Remove(RemoveMostRecentlyUsedCommand.Request request)
        {            
            var key = dictionary.GetOrAdd(request.MostRecentlyUsed.MostRecentlyUsedId, new object());

            lock (key)
                return _mediator.Send(request).GetAwaiter().GetResult();            
        }           

        [HttpGet("{mostRecentlyUsedId}")]
        public async Task<ActionResult<GetMostRecentlyUsedByIdQuery.Response>> GetById([FromRoute]GetMostRecentlyUsedByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetMostRecentlyUsedsQuery.Response>> Get()
            => await _mediator.Send(new GetMostRecentlyUsedsQuery.Request());
    }
}
