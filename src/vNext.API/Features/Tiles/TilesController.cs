using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace vNext.API.Features.Tiles
{
    [ApiController]
    [Route("api/tiles")]
    public class TilesController
    {
        private readonly IMediator _mediator;

        public TilesController(IMediator mediator)
            => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<SaveTileCommand.Response>> Add(SaveTileCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpDelete("{Tile.TileId}/{Tile.ConcurrencyVersion}")]
        public async Task Remove(RemoveTileCommand.Request request)
            => await _mediator.Send(request);            

        [HttpGet("{tileId}")]
        public async Task<ActionResult<GetTileByIdQuery.Response>> GetById(GetTileByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetTilesQuery.Response>> Get()
            => await _mediator.Send(new GetTilesQuery.Request());
    }
}
