using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace vNext.API.Features.AddressPhoneTypes
{
    [ApiController]
    [Route("api/addressPhoneTypes")]
    public class AddressPhoneTypesController
    {
        private readonly IMediator _mediator;

        public AddressPhoneTypesController(IMediator mediator)
			=> _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<SaveAddressPhoneTypeCommand.Response>> Add(SaveAddressPhoneTypeCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpDelete("{addressPhoneTypeId}/{concurrencyVersion}")]
        public async Task Remove(RemoveAddressPhoneTypeCommand.Request request)
            => await _mediator.Send(request);            

        [HttpGet("{addressPhoneTypeId}")]
        public async Task<ActionResult<GetAddressPhoneTypeByIdQuery.Response>> GetById([FromRoute]GetAddressPhoneTypeByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetAddressPhoneTypesQuery.Response>> Get()
            => await _mediator.Send(new GetAddressPhoneTypesQuery.Request());
    }
}
