using Dapper;
using FluentValidation;
using MediatR;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.DashboardTiles
{
    public class DashboardTilePinnedDeleteByDashboardTileIdCommand
    {

        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.DashboardTileId).NotNull();
            }
        }

        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response> {
            public int DashboardTileId { get; set; }
        }

        public class Response
        {
            public Response(int result) => Result = result;

            public int Result { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            public Handler( IDbConnectionManager dbConnectionManager)
                => _dbConnectionManager = dbConnectionManager;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var result = default(int);

                    using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                    {
                        connection.Open();

                        result = await Procedure.ExecuteAsync(request, connection);
                    }

                    transaction.Complete();

                    return new Response(result);
                }
            }
        }

        public static class Procedure
        {
            public static async Task<int> ExecuteAsync(Request request, System.Data.IDbConnection connection)
            {
                var dynamicParameters = new DynamicParameters();

                dynamicParameters.AddDynamicParams(new
                {
                    request.DashboardTileId
                });
                
                return await connection.ExecuteProcAsync("[Common].[ProcDashboardTilePinnedDeleteByDashboardTileId]", new { request.DashboardTileId });
                
            }
        }
    }
}
