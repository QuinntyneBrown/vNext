using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace vNext.API.Features.CountrySubDivisions
{
    [ApiController]
    [Route("api/countrySubDivisions")]
    public class CountrySubDivisionsController
    {
        private readonly IMediator _mediator;

        public CountrySubDivisionsController(IMediator mediator)
			=> _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<SaveCountrySubDivisionCommand.Response>> Add(SaveCountrySubDivisionCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpDelete("{countrySubDivisionId}/{concurrencyVersion}")]
        public async Task Remove(RemoveCountrySubDivisionCommand.Request request)
            => await _mediator.Send(request);            

        [HttpGet("{countrySubDivisionId}")]
        public async Task<ActionResult<GetCountrySubDivisionByIdQuery.Response>> GetById([FromRoute]GetCountrySubDivisionByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetCountrySubDivisionsQuery.Response>> Get()
            => await _mediator.Send(new GetCountrySubDivisionsQuery.Request());
    }
}
