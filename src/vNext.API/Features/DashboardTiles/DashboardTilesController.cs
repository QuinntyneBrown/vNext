using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace vNext.API.Features.DashboardTiles
{
    [ApiController]
    [Route("api/dashboardtiles")]
    public class DashboardTilesController
    {
        private readonly IMediator _mediator;

        public DashboardTilesController(IMediator mediator)
            => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<SaveDashboardTileCommand.Response>> Add(SaveDashboardTileCommand.Request request)
            => await _mediator.Send(request);

        
        [HttpGet("dashboard/{dashboardId}")]
        public async Task<ActionResult<GetDashboardTilesByDashboardIdQuery.Response>> GetByDashboardId([FromRoute]GetDashboardTilesByDashboardIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet("{dashboardTileId}")]
        public async Task<ActionResult<GetDashboardTileByIdQuery.Response>> GetById(GetDashboardTileByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpDelete("{DashboardTileId}/{ConcurrencyVersion}")]
        public async Task Remove([FromRoute]RemoveDashboardTileCommand.Request request)
            => await _mediator.Send(request);        
    }
}
