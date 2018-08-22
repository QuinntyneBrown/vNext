using Dapper;
using FluentValidation;
using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.AuditLogs
{
    public class SaveAuditLogCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.AuditLog.AuditLogId).NotNull();
            }
        }

        public class Request : AuthenticatedRequest, IRequest<Response> {
            public AuditLogDto AuditLog { get; set; }
        }

        public class Response
        {			
            public int AuditLogId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            private readonly IProcedure<Request, short> _procedure;
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
                        AuditLogId = await Procedure.ExecuteAsync(request,connection)
                    };
                }
            }
        }

        public class Procedure
        {
            public static async Task<short> ExecuteAsync(Request request, IDbConnection connection)
            {
                var dynamicParameters = new DynamicParameters();

                var parameterDirection = request.AuditLog.AuditLogId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                dynamicParameters.AddDynamicParams(new
                {
                    request.AuditLog.AuditLogId,
                    request.AuditLog.Code
                });

                dynamicParameters.Add("AuditLogId", request.AuditLog.AuditLogId, DbType.Int16, parameterDirection);

                await connection.ExecuteProcAsync("[Common].[ProcAuditLogSave]", dynamicParameters);

                return dynamicParameters.Get<short>("@AuditLogId");
            }
        }    
        
    }
}
