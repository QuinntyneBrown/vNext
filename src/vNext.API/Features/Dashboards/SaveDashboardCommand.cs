using Dapper;
using FluentValidation;
using MediatR;
using Newtonsoft.Json;
using System.Data;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Dashboards
{
    public class SaveDashboardCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.Dashboard.DashboardId).NotNull();
            }
        }

        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response> {
            public DashboardDto Dashboard { get; set; }
        }

        public class Response
        {			
            public int DashboardId { get; set; }
            public int ConcurrencyVersion { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            public Handler( IDbConnectionManager dbConnectionManager)
                => _dbConnectionManager = dbConnectionManager;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    return new Response()
                    {
                        DashboardId = await SaveEntityCommandHandler.Handle(request, (x, y) => Procedure.ExecuteAsync(x, y), "Dashboard", request.Dashboard.DashboardId, request.Dashboard.ConcurrencyVersion, connection)
                    };
                }                
            }
        }

        public class Procedure
        {
            public static async Task<short> ExecuteAsync(Request request, IDbConnection connection)
            {
                var dynamicParameters = new DynamicParameters();

                dynamicParameters.AddDynamicParams(new
                {
                    request.Dashboard.DashboardId,
                    request.Dashboard.Code,
                    request.Dashboard.UserId,
                    request.Dashboard.Sort,
                    Settings = JsonConvert.SerializeObject(request.Dashboard.Settings)
                });

                var parameterDirection = request.Dashboard.DashboardId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                dynamicParameters.Add("DashboardId", request.Dashboard.DashboardId, DbType.Int16, parameterDirection);

                await connection.ExecuteAsync("[Common].[ProcDashboardSave]", dynamicParameters, commandType: CommandType.StoredProcedure);

                return dynamicParameters.Get<short>("@DashboardId");
            }
        }
    }
}
