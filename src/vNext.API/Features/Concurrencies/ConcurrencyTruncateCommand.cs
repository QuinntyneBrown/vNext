using MediatR;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Concurrencies
{
    public class ConcurrencyTruncateCommand
    {        
        public class Request : IRequest<Response> { }

        public class Response { }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISqlConnectionManager _sqlConnectionManager;
            public Handler(ISqlConnectionManager sqlConnectionManager)
                => _sqlConnectionManager = sqlConnectionManager;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var connection = _sqlConnectionManager.GetConnection())
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
            public async Task<int> ExecuteAsync(Request request, SqlConnection connection)
                => await connection.ExecuteProcAsync("[Comsense].[ProcConcurrencyTruncate]");
        }
    }
}
