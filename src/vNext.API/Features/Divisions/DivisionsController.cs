using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace vNext.API.Features.Divisions
{
    [ApiController]
    [Route("api/divisions")]
    public class DivisionsController
    {
        private readonly IMediator _mediator;

        public DivisionsController(IMediator mediator)
			=> _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<SaveDivisionCommand.Response>> Add(SaveDivisionCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpDelete("{divisionId}/{concurrencyVersion}")]
        public async Task Remove(RemoveDivisionCommand.Request request)
            => await _mediator.Send(request);            

        [HttpGet("{divisionId}")]
        public async Task<ActionResult<GetDivisionByIdQuery.Response>> GetById([FromRoute]GetDivisionByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetDivisionsQuery.Response>> Get()
            => await _mediator.Send(new GetDivisionsQuery.Request());
    }
}
