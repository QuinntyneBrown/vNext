using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Warehouses
{
    public class GetWarehousesQuery
    {
        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response>
        {
        }

        public class Response
        {
            public IEnumerable<WarehouseDto> Warehouses { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            private readonly IProcedure<Request, IEnumerable<QueryProjectionDto>> _procedure;
            public Handler(IDbConnectionManager dbConnectionManager)
            {
                _dbConnectionManager = dbConnectionManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    var result = await Procedure.ExecuteAsync(request, connection);
                    return new Response()
                    {
                        //Warehouses = result.Select(x => WarehouseDto.FromWarehouse(x))
                    };
                }
            }
        }

        public class Procedure
        {
            public static async Task<IEnumerable<QueryProjectionDto>> ExecuteAsync(Request request, IDbConnection connection)
            {
                return await connection.QueryProcAsync<QueryProjectionDto>("[Product].[ProcWarehouseGetAll]");
            }
        }

        public class QueryProjectionDto
        {
            public int WarehouseId { get; set; }
            public string Code { get; set; }
            public string Description { get; set; }
            public int Status { get; set; }
            public DateTime CreatedDateTime { get; set; }
            public DateTime CreatedDate { get; set; }
            public int CreatedByUserId { get; set; }
            public float HandlingCharge { get; set; }
            public int AddressId { get; set; }
            public int NoteId { get; set; }
            public string Note { get; set; }
            public string Settings { get; set; } = "{}";
        }
    }
}
