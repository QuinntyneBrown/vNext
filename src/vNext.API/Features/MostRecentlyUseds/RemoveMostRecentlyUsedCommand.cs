using MediatR;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.MostRecentlyUseds
{
    public class RemoveMostRecentlyUsedCommand
    {
        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response>
        {
            public MostRecentlyUsedDto MostRecentlyUsed { get; set; }
        }

        public class Response
        {
            public int MostRecentlyUsedId { get; set; }
        }

        public class Handler : IRequestHandler<Request,Response>
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
                        MostRecentlyUsedId = await Procedure.ExecuteAsync(request, connection)
                    };
                }
            }
        }

        public static class Procedure
        {
            public static async Task<int> ExecuteAsync(Request request, System.Data.IDbConnection connection)
            {
                return await connection.ExecuteProcAsync("[Audit].[ProcMostRecentlyUsedDelete]", new { request.MostRecentlyUsed.MostRecentlyUsedId });
            }
        }
    }
}
