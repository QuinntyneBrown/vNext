using MediatR;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Statuses
{
    public class GetStatusesQuery
    {
        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response> { }

        public class Response
        {
            public IEnumerable<StatusDto> Statuses { get; set; }
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
                    var result = await Procedure.ExecuteAsync(request, connection);

                    return new Response()
                    {
                        Statuses = result.Select(x => StatusDto.FromStatus(x))
                    };
                }
            }
        }

        public static class Procedure
        {
            public static async Task<IEnumerable<QueryProjectionDto>> ExecuteAsync(Request request, System.Data.IDbConnection connection)
            {
                return await connection.QueryProcAsync<QueryProjectionDto>("[Comsense].[ProcStatusGetAll]");
            }
        }

        public class QueryProjectionDto
        {
            public int StatusId { get; set; }
            public string Category { get; set; }
            public int Status { get; set; }
            public string Description { get; set; }
            public int Sort { get; set; }
        }
    }
}
