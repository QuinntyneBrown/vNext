using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace vNext.API.Features.Customers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomersController
    {
        private ConcurrentDictionary<long, object> dictionary = new ConcurrentDictionary<long, object>();

        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
			=> _mediator = mediator;

        [HttpPost]
        public ActionResult<SaveCustomerCommand.Response> Add(SaveCustomerCommand.Request request)
        {            
            var key = dictionary.GetOrAdd(request.Customer.CustomerId, new object());

            lock (key)
                return _mediator.Send(request).GetAwaiter().GetResult();            
        }
        
        [HttpDelete("{customerId}/{concurrencyVersion}")]
        public ActionResult<RemoveCustomerCommand.Response> Remove(RemoveCustomerCommand.Request request)
        {            
            var key = dictionary.GetOrAdd(request.Customer.CustomerId, new object());

            lock (key)
                return _mediator.Send(request).GetAwaiter().GetResult();            
        }           

        [HttpGet("{customerId}")]
        public async Task<ActionResult<GetCustomerByIdQuery.Response>> GetById([FromRoute]GetCustomerByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetCustomersQuery.Response>> Get()
            => await _mediator.Send(new GetCustomersQuery.Request());
    }
}
