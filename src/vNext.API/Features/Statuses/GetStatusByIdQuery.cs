using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Statuses
{
    public class GetStatusByIdQuery
    {
        public class Request : AuthenticatedRequest, IRequest<Response>
        {
            public int StatusId { get; set; }
        }

        public class Response
        {
            public StatusDto Status { get; set; }
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
                    return new Response()
                    {
                        Status = StatusDto.FromStatus((await Procedure.ExecuteAsync(request, connection)))                        
                    };
                }
            }
        }

        public class Procedure
        {
            public static async Task<QueryProjectionDto> ExecuteAsync(Request request, IDbConnection connection)
            {
                return await connection.QuerySingleProcAsync<QueryProjectionDto>("[Comsense].[ProcStatusGet]", new { request.StatusId });
            }
        }

        public class QueryProjectionDto
        {
            public int StatusId { get; set; }
            public string Code { get; set; }
        }
    }
}
