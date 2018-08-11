using MediatR;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.AuditLogs
{
    public class GetAuditLogByIdQuery
    {
        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response>
        {
            public int AuditLogId { get; set; }
        }

        public class Response
        {
            public AuditLogDto AuditLog { get; set; }
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
                        AuditLog = AuditLogDto.FromAuditLog((await Procedure.ExecuteAsync(request, connection)))                        
                    };
                }
            }
        }

        public static class Procedure
        {
            public static async Task<QueryProjectionDto> ExecuteAsync(Request request, System.Data.IDbConnection connection)
            {
                return await connection.QuerySingleProcAsync<QueryProjectionDto>("[Audit].[ProcAuditLogGet]", new { request.AuditLogId });
            }
        }

        public class QueryProjectionDto
        {
            public int AuditLogId { get; set; }
            public string Code { get; set; }
        }
    }
}
