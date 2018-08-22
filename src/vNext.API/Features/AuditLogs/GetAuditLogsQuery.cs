using Dapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.AuditLogs
{
    public class GetAuditLogsQuery
    {
        public class Request : AuthenticatedRequest, IRequest<Response> {
            public string Domain { get; set; }
            public int Id { get; set; }
            public int UserId { get; set; }
            public DateTime FromAuditDate { get; set; }
            public DateTime ToAuditDate { get; set; }
        }

        public class Response
        {
            public IEnumerable<AuditLogDto> AuditLogs { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            private readonly IProcedure<Request, IEnumerable<QueryProjectionDto>> _procedure;
            public Handler(IDbConnectionManager dbConnectionManager, IProcedure<Request, IEnumerable<QueryProjectionDto>> procedure)
            {
                _dbConnectionManager = dbConnectionManager;
                _procedure = procedure;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    var result = await _procedure.ExecuteAsync(request, connection);

                    return new Response()
                    {
                        AuditLogs = result.Select(x => AuditLogDto.FromAuditLog(x))
                    };
                }
            }
        }

        public class Procedure: IProcedure<Request, IEnumerable<QueryProjectionDto>>
        {
            public async Task<IEnumerable<QueryProjectionDto>> ExecuteAsync(Request request, IDbConnection connection)
            {
                var dynamicParameters = new DynamicParameters();

                dynamicParameters.AddDynamicParams(new
                {
                    request.Domain,
                    request.Id,
                    request.UserId,
                    request.FromAuditDate,
                    request.ToAuditDate
                });

                return await connection.QueryProcAsync<QueryProjectionDto>("[Audit].[ProcAuditLogGet]",dynamicParameters);
            }
        }

        public class QueryProjectionDto
        {
            public int AuditLogId { get; set; }
            public string Domain { get; set; }
            public int UserId { get; set; }
            public string Operation { get; set; }
            public DateTime AuditDateTime { get; set; }
            public DateTime AuditDate { get; set; }
            public short Status { get; set; }
            public string Info { get; set; }
            public int Id { get; set; }
            public int NoteId { get; set; }
            public string Note { get; set; }
        }
    }
}
