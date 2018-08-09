using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace vNext.API.Features.Warehouses
{
    [Authorize]
    [ApiController]
    [Route("api/warehouses")]
    public class WarehousesController
    {
        private readonly IMediator _mediator;

        public WarehousesController(IMediator mediator)
			=> _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<SaveWarehouseCommand.Response>> Add(SaveWarehouseCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpDelete("{warehouseId}/{concurrencyVersion}")]
        public async Task Remove(RemoveWarehouseCommand.Request request)
            => await _mediator.Send(request);            

        [HttpGet("{warehouseId}")]
        public async Task<ActionResult<GetWarehouseByIdQuery.Response>> GetById([FromRoute]GetWarehouseByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetWarehousesQuery.Response>> Get()
            => await _mediator.Send(new GetWarehousesQuery.Request());
    }
}
