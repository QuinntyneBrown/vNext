using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace vNext.API.Features.AddressPhones
{
    [ApiController]
    [Route("api/addressPhones")]
    public class AddressPhonesController
    {
        private readonly IMediator _mediator;

        public AddressPhonesController(IMediator mediator)
			=> _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<SaveAddressPhoneCommand.Response>> Add(SaveAddressPhoneCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpDelete("{addressPhoneId}/{concurrencyVersion}")]
        public async Task Remove(RemoveAddressPhoneCommand.Request request)
            => await _mediator.Send(request);            

        [HttpGet("{addressPhoneId}")]
        public async Task<ActionResult<GetAddressPhoneByIdQuery.Response>> GetById([FromRoute]GetAddressPhoneByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetAddressPhonesQuery.Response>> Get()
            => await _mediator.Send(new GetAddressPhonesQuery.Request());
    }
}
