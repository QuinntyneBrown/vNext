using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace vNext.API.Features.SalesOrders
{
    [Authorize]
    [ApiController]
    [Route("api/salesOrders")]
    public class SalesOrdersController: BaseController
    {
        public SalesOrdersController(IMediator mediator)
            : base("SalesOrder",mediator) { }

        [HttpPost]
        public ActionResult<SaveSalesOrderCommand.Response> Add(SaveSalesOrderCommand.Request request)
            => Submit(request.SalesOrder.SalesOrderId, request);
        
        [HttpDelete("{salesOrderId}/{concurrencyVersion}")]
        public ActionResult<RemoveSalesOrderCommand.Response> Remove(RemoveSalesOrderCommand.Request request)
            => Submit(request.SalesOrder.SalesOrderId, request);          

        [HttpGet("{salesOrderId}")]
        public async Task<ActionResult<GetSalesOrderByIdQuery.Response>> GetById([FromRoute]GetSalesOrderByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetSalesOrdersQuery.Response>> Get()
            => await _mediator.Send(new GetSalesOrdersQuery.Request());
    }
}
