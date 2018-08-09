using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace vNext.API.Features.Territories
{
    [ApiController]
    [Route("api/territories")]
    public class TerritoriesController
    {
        private readonly IMediator _mediator;

        public TerritoriesController(IMediator mediator)
			=> _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<SaveTerritoryCommand.Response>> Add(SaveTerritoryCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpDelete("{territoryId}/{concurrencyVersion}")]
        public async Task Remove(RemoveTerritoryCommand.Request request)
            => await _mediator.Send(request);            

        [HttpGet("{territoryId}")]
        public async Task<ActionResult<GetTerritoryByIdQuery.Response>> GetById([FromRoute]GetTerritoryByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetTerritoriesQuery.Response>> Get()
            => await _mediator.Send(new GetTerritoriesQuery.Request());
    }
}
