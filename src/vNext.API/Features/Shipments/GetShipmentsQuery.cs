using MediatR;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Shipments
{
    public class GetShipmentsQuery
    {
        public class Request : AuthenticatedRequest, IRequest<Response> { }

        public class Response
        {
            public IEnumerable<ShipmentDto> Shipments { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            private readonly IProcedure<Request, IEnumerable<QueryProjectionDto>> _procedure;
            public Handler(IDbConnectionManager dbConnectionManager, IProcedure<Request, IEnumerable<QueryProjectionDto>> procedure)
            {
                _dbConnectionManager = dbConnectionManager;
                _procedure = procedure;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    var result = await _procedure.ExecuteAsync(request, connection);

                    return new Response()
                    {
                        Shipments = result.Select(x => ShipmentDto.FromShipment(x))
                    };
                }
            }
        }

        public class Procedure: IProcedure<Request, IEnumerable<QueryProjectionDto>>
        {
            public async Task<IEnumerable<QueryProjectionDto>> ExecuteAsync(Request request, IDbConnection connection)
            {
                return await connection.QueryProcAsync<QueryProjectionDto>("[Shipment].[ProcShipmentGetAll]");
            }
        }

        public class QueryProjectionDto
        {
            public int ShipmentId { get; set; }
            public string Code { get; set; }
        }
    }
}
