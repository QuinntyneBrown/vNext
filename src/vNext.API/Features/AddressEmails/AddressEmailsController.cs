using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace vNext.API.Features.AddressEmails
{
    [ApiController]
    [Route("api/addressEmails")]
    public class AddressEmailsController
    {
        private readonly IMediator _mediator;

        public AddressEmailsController(IMediator mediator)
			=> _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<SaveAddressEmailCommand.Response>> Add(SaveAddressEmailCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpDelete("{addressEmailId}/{concurrencyVersion}")]
        public async Task Remove(RemoveAddressEmailCommand.Request request)
            => await _mediator.Send(request);            

        [HttpGet("{addressEmailId}")]
        public async Task<ActionResult<GetAddressEmailByIdQuery.Response>> GetById([FromRoute]GetAddressEmailByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetAddressEmailsQuery.Response>> Get()
            => await _mediator.Send(new GetAddressEmailsQuery.Request());
    }
}
