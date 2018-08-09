using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace vNext.API.Features.AddressEmailTypes
{
    [ApiController]
    [Route("api/addressEmailTypes")]
    public class AddressEmailTypesController
    {
        private readonly IMediator _mediator;

        public AddressEmailTypesController(IMediator mediator)
			=> _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<SaveAddressEmailTypeCommand.Response>> Add(SaveAddressEmailTypeCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpDelete("{addressEmailTypeId}/{concurrencyVersion}")]
        public async Task Remove(RemoveAddressEmailTypeCommand.Request request)
            => await _mediator.Send(request);            

        [HttpGet("{addressEmailTypeId}")]
        public async Task<ActionResult<GetAddressEmailTypeByIdQuery.Response>> GetById([FromRoute]GetAddressEmailTypeByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetAddressEmailTypesQuery.Response>> Get()
            => await _mediator.Send(new GetAddressEmailTypesQuery.Request());
    }
}
