using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Concurrencies
{
    public class ConcurrencyTruncateCommand
    {        
        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response> { }

        public class Response { }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            public Handler(IDbConnectionManager dbConnectionManager)
                => _dbConnectionManager = dbConnectionManager;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                    {
                        connection.Open();

                        await new Procedure().ExecuteAsync(request, connection);
                    }

                    transaction.Complete();

                    return new Response();
                }
            }
        }

        public class Procedure
        {
            public async Task<int> ExecuteAsync(Request request, IDbConnection connection)
                => await connection.ExecuteProcAsync("[Comsense].[ProcConcurrencyTruncate]");
        }
    }
}
