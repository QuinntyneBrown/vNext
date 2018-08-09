using MediatR;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.MostRecentlyUseds
{
    public class GetMostRecentlyUsedByIdQuery
    {
        public class Request : IRequest<Response>
        {
            public int MostRecentlyUsedId { get; set; }
        }

        public class Response
        {
            public MostRecentlyUsedDto MostRecentlyUsed { get; set; }
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
                        MostRecentlyUsed = MostRecentlyUsedDto.FromMostRecentlyUsed((await Procedure.ExecuteAsync(request, connection)))                        
                    };
                }
            }
        }

        public static class Procedure
        {
            public static async Task<QueryProjectionDto> ExecuteAsync(Request request, SqlConnection connection)
            {
                return await connection.QuerySingleProcAsync<QueryProjectionDto>("[Audit].[ProcMostRecentlyUsedGet]", new { request.MostRecentlyUsedId });
            }
        }

        public class QueryProjectionDto
        {
            public int MostRecentlyUsedId { get; set; }
            public string Code { get; set; }
        }
    }
}
