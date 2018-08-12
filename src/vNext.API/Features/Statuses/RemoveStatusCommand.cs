using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Statuses
{
    public class RemoveStatusCommand
    {
        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response>
        {
            public int StatusId { get; set; }
            public int ConcurrencyVersion { get; set; }
        }

        public class Response
        {
            public int StatusId { get; set; }
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
                        StatusId = await Procedure.ExecuteAsync(request, connection)
                    };
                }
            }
        }

        public static class Procedure
        {
            public static async Task<int> ExecuteAsync(Request request, IDbConnection connection)
            {
                return await connection.ExecuteProcAsync("[Comsense].[ProcStatusDelete]", new { request.StatusId });
            }
        }
    }
}
