using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.SalesOrders
{
    public class GetSalesOrderByIdQuery
    {
        public class Request : AuthenticatedRequest, IRequest<Response>
        {
            public int SalesOrderId { get; set; }
        }

        public class Response
        {
            public SalesOrderDto SalesOrder { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            private readonly IProcedure<Request, QueryProjectionDto> _procedure;
            public Handler(IDbConnectionManager dbConnectionManager, IProcedure<Request, QueryProjectionDto> procedure)
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
                        SalesOrder = SalesOrderDto.FromSalesOrder((await _procedure.ExecuteAsync(request, connection)))                        
                    };
                }
            }
        }

        public class Procedure: IProcedure<Request,QueryProjectionDto>
        {
            public async Task<QueryProjectionDto> ExecuteAsync(Request request, IDbConnection connection)
            {
                return await connection.QuerySingleProcAsync<QueryProjectionDto>("[SalesOrder].[ProcSalesOrderGet]", new { request.SalesOrderId });
            }
        }

        public class QueryProjectionDto
        {
            public int SalesOrderId { get; set; }
            public string Code { get; set; }
        }
    }
}
