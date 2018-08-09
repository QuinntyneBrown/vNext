using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace vNext.CompanyService.CompanyAddresses
{
    [ApiController]
    [Route("api/companyAddresses")]
    public class CompanyAddressesController
    {
        private readonly IMediator _mediator;

        public CompanyAddressesController(IMediator mediator)
			=> _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<SaveCompanyAddressCommand.Response>> Add(SaveCompanyAddressCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpDelete("{companyAddressId}/{concurrencyVersion}")]
        public async Task Remove(RemoveCompanyAddressCommand.Request request)
            => await _mediator.Send(request);            

        [HttpGet("{companyAddressId}")]
        public async Task<ActionResult<GetCompanyAddressByIdQuery.Response>> GetById([FromRoute]GetCompanyAddressByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetCompanyAddressesQuery.Response>> Get()
            => await _mediator.Send(new GetCompanyAddressesQuery.Request());
    }
}
