using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Notes
{
    public class GetNoteByIdQuery
    {
        public class Request : AuthenticatedRequest, IRequest<Response>
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
            {
                _dbConnectionManager = dbConnectionManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {                    
                    return new Response()
                    {
                        Note = await Procedure.ExecuteAsync(request,connection)
                    };
                }
            }
        }

        public class Procedure {
            public static async Task<NoteDto> ExecuteAsync(Request request, IDbConnection connection)
            {
                return await connection.QuerySingleProcAsync<NoteDto>("[Comsense].[ProcNoteGet]", new { request.NoteId });
            }
        }
    }
}
