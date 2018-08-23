using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.SalesOrders
{
    public class RemoveSalesOrderCommand
    {
        public class Request : AuthenticatedRequest, IRequest<Response>
        {
            public SalesOrderDto SalesOrder { get; set; }
        }

        public class Response
        {
            public int SalesOrderId { get; set; }
        }

        public class Handler : IRequestHandler<Request,Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            private readonly IProcedure<Request, short> _procedure;
            public Handler(IDbConnectionManager dbConnectionManager, IProcedure<Request, short> procedure)
            {
                _dbConnectionManager = dbConnectionManager;
                _procedure = procedure;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    return new Response()
                    {
                        SalesOrderId = await _procedure.ExecuteAsync(request, connection)
                    };
                }
            }
        }

        public class Procedure
        {
            public async Task<int> ExecuteAsync(Request request, IDbConnection connection)
            {
                return await connection.ExecuteProcAsync("[SalesOrder].[ProcSalesOrderDelete]", new { request.SalesOrder.SalesOrderId });
            }
        }
    }
}
