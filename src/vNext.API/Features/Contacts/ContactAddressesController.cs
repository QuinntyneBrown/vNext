using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace vNext.ContactService.ContactAddresses
{
    [ApiController]
    [Route("api/contactAddresses")]
    public class ContactAddressesController
    {
        private readonly IMediator _mediator;

        public ContactAddressesController(IMediator mediator)
			=> _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<SaveContactAddressCommand.Response>> Add(SaveContactAddressCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpDelete("{contactAddressId}/{concurrencyVersion}")]
        public async Task Remove(RemoveContactAddressCommand.Request request)
            => await _mediator.Send(request);            

        [HttpGet("{contactAddressId}")]
        public async Task<ActionResult<GetContactAddressByIdQuery.Response>> GetById([FromRoute]GetContactAddressByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetContactAddressesQuery.Response>> Get()
            => await _mediator.Send(new GetContactAddressesQuery.Request());
    }
}
