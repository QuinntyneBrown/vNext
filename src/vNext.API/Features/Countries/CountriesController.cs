using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace vNext.API.Features.Countries
{
    [ApiController]
    [Route("api/countries")]
    public class CountriesController
    {
        private readonly IMediator _mediator;

        public CountriesController(IMediator mediator)
			=> _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<SaveCountryCommand.Response>> Add(SaveCountryCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpDelete("{countryId}/{concurrencyVersion}")]
        public async Task Remove(RemoveCountryCommand.Request request)
            => await _mediator.Send(request);            

        [HttpGet("{countryId}")]
        public async Task<ActionResult<GetCountryByIdQuery.Response>> GetById([FromRoute]GetCountryByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetCountriesQuery.Response>> Get()
            => await _mediator.Send(new GetCountriesQuery.Request());
    }
}
