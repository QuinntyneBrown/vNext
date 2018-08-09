using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace vNext.API.Features.Dashboards
{
    [ApiController]
    [Route("api/dashboards")]
    public class DashboardsController
    {
        private readonly IMediator _mediator;

        public DashboardsController(IMediator mediator)
            => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<SaveDashboardCommand.Response>> Add(SaveDashboardCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpDelete("{Dashboard.DashboardId}/{Dashboard.ConcurrencyVersion}")]
        public async Task Remove([FromRoute]RemoveTaxCommand.Request request)
            => await _mediator.Send(request);

        [HttpGet("{dashboardId}")]
        public async Task<ActionResult<GetDashboardByIdQuery.Response>> GetById([FromRoute]GetDashboardByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<DashboardGetByUserIdQuery.Response>> GetByUserId([FromRoute]DashboardGetByUserIdQuery.Request request)
            => await _mediator.Send(request);
    }
}
