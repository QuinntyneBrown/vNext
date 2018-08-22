using Dapper;
using FluentValidation;
using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
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
            private readonly IProcedure<Request, short> _procedure;
            public Handler(IDbConnectionManager dbConnectionManager)
            {
                _dbConnectionManager = dbConnectionManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {

                var result = default(int);

                using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    connection.Open();

                    result = await Procedure.ExecuteAsync(request, connection);
                }

                return new Response(result);
            }
        }

        public class Procedure
        {
            public static async Task<int> ExecuteAsync(Request request, IDbConnection connection)
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
