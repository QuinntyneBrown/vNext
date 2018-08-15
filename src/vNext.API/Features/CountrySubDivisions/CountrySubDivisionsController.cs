using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace vNext.API.Features.CountrySubdivisions
{
    [ApiController]
    [Route("api/countrySubdivisions")]
    public class CountrySubdivisionsController
    {
        private readonly IMediator _mediator;

        public CountrySubdivisionsController(IMediator mediator)
			=> _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<SaveCountrySubdivisionCommand.Response>> Add(SaveCountrySubdivisionCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpDelete("{countrySubdivisionId}/{concurrencyVersion}")]
        public async Task Remove(RemoveCountrySubdivisionCommand.Request request)
            => await _mediator.Send(request);            

        [HttpGet("{countrySubdivisionId}")]
        public async Task<ActionResult<GetCountrySubdivisionByIdQuery.Response>> GetById([FromRoute]GetCountrySubdivisionByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetCountrySubdivisionsQuery.Response>> Get()
            => await _mediator.Send(new GetCountrySubdivisionsQuery.Request());
    }
}
