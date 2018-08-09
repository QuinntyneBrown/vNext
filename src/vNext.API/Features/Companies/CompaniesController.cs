using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace vNext.API.Features.Companies
{
    [ApiController]
    [Route("api/companies")]
    public class CompaniesController
    {
        private readonly IMediator _mediator;

        public CompaniesController(IMediator mediator)
			=> _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<SaveCompanyCommand.Response>> Add(SaveCompanyCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpDelete("{companyId}/{concurrencyVersion}")]
        public async Task Remove(RemoveCompanyCommand.Request request)
            => await _mediator.Send(request);            

        [HttpGet("{companyId}")]
        public async Task<ActionResult<GetCompanyByIdQuery.Response>> GetById([FromRoute]GetCompanyByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetCompaniesQuery.Response>> Get()
            => await _mediator.Send(new GetCompaniesQuery.Request());
    }
}
