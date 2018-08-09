using MediatR;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;
using vNext.Core.Models;

namespace vNext.API.Features.Warehouses
{
    public class GetWarehouseByIdQuery
    {
        public class Request : IRequest<Response>
        {
            public int WarehouseId { get; set; }
        }

        public class Response
        {
            public WarehouseDto Warehouse { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISqlConnectionManager _sqlConnectionManager;
            public Handler(ISqlConnectionManager sqlConnectionManager)
                => _sqlConnectionManager = sqlConnectionManager;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _sqlConnectionManager.GetConnection())
                {
                    var warehouse = await connection.QuerySingleProcAsync<Warehouse>("[Product].[ProcWarehouseGet]", new { request.WarehouseId });

                    return new Response()
                    {
                        Warehouse = WarehouseDto.FromWarehouse(warehouse)
                    };
                }
            }
        }
    }
}
