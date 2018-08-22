using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.MostRecentlyUseds
{
    public class GetMostRecentlyUsedByIdQuery
    {
        public class Request : AuthenticatedRequest, IRequest<Response>
        {
            public int MostRecentlyUsedId { get; set; }
        }

        public class Response
        {
            public MostRecentlyUsedDto MostRecentlyUsed { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            private readonly IProcedure<Request, QueryProjectionDto> _procedure;
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
                        MostRecentlyUsed = MostRecentlyUsedDto.FromMostRecentlyUsed((await Procedure.ExecuteAsync(request, connection)))                        
                    };
                }
            }
        }

        public class Procedure
        {
            public static async Task<QueryProjectionDto> ExecuteAsync(Request request, IDbConnection connection)
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
