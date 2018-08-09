using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace vNext.API.Features.Taxes
{
    [ApiController]
    [Route("api/taxes")]
    public class TaxesController
    {
        private ConcurrentDictionary<long, object> dictionary = new ConcurrentDictionary<long, object>();

        private readonly IMediator _mediator;

        public TaxesController(IMediator mediator)
			=> _mediator = mediator;

        [HttpPost]
        public ActionResult<SaveTaxCommand.Response> Add(SaveTaxCommand.Request request)
        {            
            var key = dictionary.GetOrAdd(request.Tax.TaxId, new object());

            lock (key)
                return _mediator.Send(request).GetAwaiter().GetResult();            
        }
        
        [HttpDelete("{taxId}/{concurrencyVersion}")]
        public ActionResult<RemoveTaxCommand.Response> Remove(RemoveTaxCommand.Request request)
        {            
            var key = dictionary.GetOrAdd(request.Tax.TaxId, new object());

            lock (key)
                return _mediator.Send(request).GetAwaiter().GetResult();            
        }           

        [HttpGet("{taxId}")]
        public async Task<ActionResult<GetTaxByIdQuery.Response>> GetById([FromRoute]GetTaxByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetTaxesQuery.Response>> Get()
            => await _mediator.Send(new GetTaxesQuery.Request());
    }
}
