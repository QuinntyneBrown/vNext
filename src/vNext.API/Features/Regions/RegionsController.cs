using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Regions
{
    [Authorize]
    [ApiController]
    [Route("api/regions")]
    public class RegionsController: BaseController
    {
        public RegionsController(IMediator mediator)
            : base("Region",mediator) { }

        [HttpPost]
        public ActionResult<SaveRegionCommand.Response> Add(SaveRegionCommand.Request request)
            => Submit(request.Region.RegionId, request);

        [HttpDelete("{regionId}/{concurrencyVersion}")]
        public ActionResult<RemoveRegionCommand.Response> Remove([FromRoute]RemoveRegionCommand.Request request)
            => Submit(request.RegionId, request);

        [HttpGet("{regionId}")]
        public async Task<ActionResult<GetRegionByIdQuery.Response>> GetById([FromRoute]GetRegionByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetRegionsQuery.Response>> Get()
            => await _mediator.Send(new GetRegionsQuery.Request());
    }
}
