using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace vNext.API.Features.Statuses
{
    [Authorize]
    [ApiController]
    [Route("api/statuses")]
    public class StatusesController
    {
        private ConcurrentDictionary<long, object> dictionary = new ConcurrentDictionary<long, object>();

        private readonly IMediator _mediator;

        public StatusesController(IMediator mediator)
			=> _mediator = mediator;

        [HttpPost]
        public ActionResult<SaveStatusCommand.Response> Add(SaveStatusCommand.Request request)
        {            
            var key = dictionary.GetOrAdd(request.Status.StatusId, new object());

            lock (key)
                return _mediator.Send(request).GetAwaiter().GetResult();            
        }
        
        [HttpDelete("{statusId}/{concurrencyVersion}")]
        public ActionResult<RemoveStatusCommand.Response> Remove(RemoveStatusCommand.Request request)
        {            
            var key = dictionary.GetOrAdd(request.StatusId, new object());

            lock (key)
                return _mediator.Send(request).GetAwaiter().GetResult();            
        }           

        [HttpGet("{statusId}")]
        public async Task<ActionResult<GetStatusByIdQuery.Response>> GetById([FromRoute]GetStatusByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetStatusesQuery.Response>> Get()
            => await _mediator.Send(new GetStatusesQuery.Request());
    }
}
