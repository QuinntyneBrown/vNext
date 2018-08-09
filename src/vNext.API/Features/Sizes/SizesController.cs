using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace vNext.API.Features.Sizes
{
    [ApiController]
    [Route("api/sizes")]
    public class SizesController
    {
        private ConcurrentDictionary<long, object> dictionary = new ConcurrentDictionary<long, object>();

        private readonly IMediator _mediator;

        public SizesController(IMediator mediator)
			=> _mediator = mediator;

        [HttpPost]
        public ActionResult<SaveSizeCommand.Response> Add(SaveSizeCommand.Request request)
        {            
            var key = dictionary.GetOrAdd(request.Size.SizeId, new object());

            lock (key)
                return _mediator.Send(request).GetAwaiter().GetResult();            
        }
        
        [HttpDelete("{sizeId}/{concurrencyVersion}")]
        public ActionResult<RemoveSizeCommand.Response> Remove(RemoveSizeCommand.Request request)
        {            
            var key = dictionary.GetOrAdd(request.Size.SizeId, new object());

            lock (key)
                return _mediator.Send(request).GetAwaiter().GetResult();            
        }           

        [HttpGet("{sizeId}")]
        public async Task<ActionResult<GetSizeByIdQuery.Response>> GetById([FromRoute]GetSizeByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetSizesQuery.Response>> Get()
            => await _mediator.Send(new GetSizesQuery.Request());
    }
}
