using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace vNext.API.Features.Discounts
{
    [ApiController]
    [Route("api/discounts")]
    public class DiscountsController
    {
        private ConcurrentDictionary<long, object> dictionary = new ConcurrentDictionary<long, object>();

        private readonly IMediator _mediator;

        public DiscountsController(IMediator mediator)
			=> _mediator = mediator;

        [HttpPost]
        public ActionResult<SaveDiscountCommand.Response> Add(SaveDiscountCommand.Request request)
        {            
            var key = dictionary.GetOrAdd(request.Discount.DiscountId, new object());

            lock (key)
                return _mediator.Send(request).GetAwaiter().GetResult();            
        }
        
        [HttpDelete("{discountId}/{concurrencyVersion}")]
        public ActionResult<RemoveDiscountCommand.Response> Remove(RemoveDiscountCommand.Request request)
        {            
            var key = dictionary.GetOrAdd(request.Discount.DiscountId, new object());

            lock (key)
                return _mediator.Send(request).GetAwaiter().GetResult();            
        }           

        [HttpGet("{discountId}")]
        public async Task<ActionResult<GetDiscountByIdQuery.Response>> GetById([FromRoute]GetDiscountByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetDiscountsQuery.Response>> Get()
            => await _mediator.Send(new GetDiscountsQuery.Request());
    }
}
