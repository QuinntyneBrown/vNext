using MediatR;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace VNext.API.Features.Documents
{
    public class DocumentGetQuery
    {
        public class Request : IRequest<Response> { }

        public class Response
        {
            public IEnumerable<DocumentDto> Documents { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISqlConnectionManager _sqlConnectionManager;
            public Handler(ISqlConnectionManager sqlConnectionManager)
			    => _sqlConnectionManager = sqlConnectionManager;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _sqlConnectionManager.GetConnection())
                {
                    return new Response()
                    {
                        Documents = (await Procedure.ExecuteAsync(request, connection))
                        .Select(x => DocumentDto.FromDocument(x)) as IEnumerable<DocumentDto>
                    };
                }
            }
        }

        public static class Procedure
        {
            public static async Task<IEnumerable<QueryProjectionDto>> ExecuteAsync(Request request, SqlConnection connection)
            {
                return await connection.QueryProcAsync<QueryProjectionDto>("[Comsense].[ProcDocumentGet]");
            }
        }

        public class QueryProjectionDto
        {
            public int DocumentId { get; set; }
            public byte Document { get; set; }
        }
    }
}
