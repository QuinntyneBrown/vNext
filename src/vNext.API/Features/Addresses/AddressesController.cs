using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace vNext.API.Features.Addresses
{
    [ApiController]
    [Route("api/addresses")]
    public class AddressesController
    {
        private readonly IMediator _mediator;

        public AddressesController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<SaveAddressCommand.Response>> Save(SaveAddressCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpGet("{addressId}")]
        public async Task<ActionResult<GetAddressByIdQuery.Response>> GetById([FromRoute]GetAddressByIdQuery.Request request)
            => await _mediator.Send(request);
    }
}
