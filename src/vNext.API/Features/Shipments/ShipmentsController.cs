using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace vNext.API.Features.Shipments
{
    [Authorize]
    [ApiController]
    [Route("api/shipments")]
    public class ShipmentsController: BaseController
    {
        public ShipmentsController(IMediator mediator)
            : base("Shipment",mediator) { }

        [HttpPost]
        public ActionResult<SaveShipmentCommand.Response> Add(SaveShipmentCommand.Request request)
            => Submit(request.Shipment.ShipmentId, request);
        
        [HttpDelete("{shipmentId}/{concurrencyVersion}")]
        public ActionResult<RemoveShipmentCommand.Response> Remove(RemoveShipmentCommand.Request request)
            => Submit(request.Shipment.ShipmentId, request);          

        [HttpGet("{shipmentId}")]
        public async Task<ActionResult<GetShipmentByIdQuery.Response>> GetById([FromRoute]GetShipmentByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetShipmentsQuery.Response>> Get()
            => await _mediator.Send(new GetShipmentsQuery.Request());
    }
}
