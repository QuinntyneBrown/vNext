using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Warehouses
{
    public class GetWarehouseByIdQuery
    {
        public class Request : AuthenticatedRequest, IRequest<Response>
        {
            public int WarehouseId { get; set; }
        }

        public class Response
        {
            public WarehouseDto Warehouse { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            private readonly IProcedure<Request, WarehouseDto> _procedure;
            public Handler(IDbConnectionManager dbConnectionManager, IProcedure<Request,WarehouseDto> procedure)
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
                        Warehouse = await _procedure.ExecuteAsync(request,connection)
                    };
                }
            }
        }

        public class Procedure : IProcedure<Request, WarehouseDto>
        {
            public async Task<WarehouseDto> ExecuteAsync(Request request, IDbConnection connection)
            {
                return await connection.QuerySingleProcAsync<WarehouseDto>("[Product].[ProcWarehouseGet]", new { request.WarehouseId });
            }
        }
    }
}
