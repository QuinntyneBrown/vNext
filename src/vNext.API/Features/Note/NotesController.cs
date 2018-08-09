using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace vNext.API.Features.Notes
{
    [ApiController]
    [Route("api/notes")]
    public class NotesController
    {
        private readonly IMediator _mediator;

        public NotesController(IMediator mediator)
			=> _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<SaveNoteCommand.Response>> Add(SaveNoteCommand.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        [HttpGet("id/{note}")]
        public async Task<ActionResult<GetNoteIdByNoteQuery.Response>> GetNoteIdByNote([FromRoute]GetNoteIdByNoteQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet("{noteId}")]
        public async Task<ActionResult<GetNoteByIdQuery.Response>> GetById([FromRoute]GetNoteByIdQuery.Request request)
            => await _mediator.Send(request);
    }
}
