using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace vNext.API.Features.Contacts
{
    [ApiController]
    [Route("api/contacts")]
    public class ContactsController: BaseController
    {        
        public ContactsController(IMediator mediator)
            : base("Contact", mediator) { }

        [HttpPost]
        public async Task<ActionResult<SaveContactCommand.Response>> Add(SaveContactCommand.Request request)
            => await _mediator.Send(request);

        [HttpDelete("{ContactId}/{ConcurrencyVersion}")]
        public ActionResult<RemoveContactCommand.Response> Remove([FromRoute]RemoveContactCommand.Request request)
            => Submit(request.ContactId, request);

        [HttpGet]
        public async Task<ActionResult<ContactGetAllQuery.Response>> Get()
            => await _mediator.Send(new ContactGetAllQuery.Request());

        [HttpGet("{contactId}")]
        public async Task<ActionResult<GetContactByIdQuery.Response>> GetById([FromRoute]GetContactByIdQuery.Request request)
            => await _mediator.Send(request);        
    }
}
