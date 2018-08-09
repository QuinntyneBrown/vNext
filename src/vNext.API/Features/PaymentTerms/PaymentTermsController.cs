using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace vNext.API.Features.PaymentTerms
{
    [ApiController]
    [Route("api/paymentTerms")]
    public class PaymentTermsController
    {
        private ConcurrentDictionary<long, object> dictionary = new ConcurrentDictionary<long, object>();

        private readonly IMediator _mediator;

        public PaymentTermsController(IMediator mediator)
			=> _mediator = mediator;

        [HttpPost]
        public ActionResult<SavePaymentTermCommand.Response> Add(SavePaymentTermCommand.Request request)
        {            
            var key = dictionary.GetOrAdd(request.PaymentTerm.PaymentTermId, new object());

            lock (key)
                return _mediator.Send(request).GetAwaiter().GetResult();            
        }
        
        [HttpDelete("{paymentTermId}/{concurrencyVersion}")]
        public ActionResult<RemovePaymentTermCommand.Response> Remove(RemovePaymentTermCommand.Request request)
        {            
            var key = dictionary.GetOrAdd(request.PaymentTerm.PaymentTermId, new object());

            lock (key)
                return _mediator.Send(request).GetAwaiter().GetResult();            
        }           

        [HttpGet("{paymentTermId}")]
        public async Task<ActionResult<GetPaymentTermByIdQuery.Response>> GetById([FromRoute]GetPaymentTermByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetPaymentTermsQuery.Response>> Get()
            => await _mediator.Send(new GetPaymentTermsQuery.Request());
    }
}
