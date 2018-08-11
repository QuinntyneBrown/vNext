using MediatR;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;
using vNext.Core.Models;

namespace vNext.API.Features.Notes
{
    public class GetNoteByIdQuery
    {
        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response>
        {
            public int NoteId { get; set; }
        }

        public class Response
        {
            public NoteDto Note { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            public Handler(IDbConnectionManager dbConnectionManager)
                => _dbConnectionManager = dbConnectionManager;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    var note = await connection.QuerySingleProcAsync<Core.Models.Note>("[Comsense].[ProcNoteGet]", new { request.NoteId });

                    return new Response()
                    {
                        Note = NoteDto.FromNote(note)
                    };
                }
            }
        }
    }
}
