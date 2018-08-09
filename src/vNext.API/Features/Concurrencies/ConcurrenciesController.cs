using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace vNext.API.Features.Concurrencies
{
    [ApiController]
    [Route("api/concurrencies")]
    public class ConcurrenciesController
    {
        private readonly IMediator _mediator;

        public ConcurrenciesController(IMediator mediator)
			=> _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<ConcurrencySaveCommand.Response>> Save(ConcurrencySaveCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpGet]
        public async Task<ActionResult<GetConcurrenciesQuery.Response>> Get()
            => await _mediator.Send(new GetConcurrenciesQuery.Request());

        [HttpGet]
        [Route("domain/{domain}/id/{id}/version/{version}")]
        public async Task<ConcurrencyGetVersionByDomainAndIdQuery.Response> GetVersionByDomainAndId([FromRoute]ConcurrencyGetVersionByDomainAndIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpDelete]
        public async Task<ActionResult<ConcurrencyTruncateCommand.Response>> Delete()
            => await _mediator.Send(new ConcurrencyTruncateCommand.Request());
    }
}
