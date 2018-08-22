using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Concurrencies
{
    public class ConcurrencyTruncateCommand
    {        
        public class Request : AuthenticatedRequest, IRequest<Response> { }

        public class Response { }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            public Handler(IDbConnectionManager dbConnectionManager)
            {
                _dbConnectionManager = dbConnectionManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    connection.Open();

                    await Procedure.ExecuteAsync(request, connection);
                }

                return new Response();
            }
        }

        public class Procedure
        {
            public static async Task<int> ExecuteAsync(Request request, IDbConnection connection)
                => await connection.ExecuteProcAsync("[Comsense].[ProcConcurrencyTruncate]");
        }
    }
}
