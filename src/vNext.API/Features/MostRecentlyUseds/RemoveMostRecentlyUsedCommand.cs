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
        public class Request : IRequest<Response>
        {
            public MostRecentlyUsedDto MostRecentlyUsed { get; set; }
        }

        public class Response
        {
            public int MostRecentlyUsedId { get; set; }
        }

        public class Handler : IRequestHandler<Request,Response>
        {
            private readonly ISqlConnectionManager _sqlConnectionManager;
            public Handler(ISqlConnectionManager sqlConnectionManager)
                => _sqlConnectionManager = sqlConnectionManager;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _sqlConnectionManager.GetConnection())
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
            public static async Task<int> ExecuteAsync(Request request, SqlConnection connection)
            {
                return await connection.ExecuteProcAsync("[Audit].[ProcMostRecentlyUsedDelete]", new { request.MostRecentlyUsed.MostRecentlyUsedId });
            }
        }
    }
}
