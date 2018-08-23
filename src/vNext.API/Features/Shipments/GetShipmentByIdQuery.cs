using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Shipments
{
    public class GetShipmentByIdQuery
    {
        public class Request : AuthenticatedRequest, IRequest<Response>
        {
            public int ShipmentId { get; set; }
        }

        public class Response
        {
            public ShipmentDto Shipment { get; set; }
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
                        Shipment = ShipmentDto.FromShipment((await _procedure.ExecuteAsync(request, connection)))                        
                    };
                }
            }
        }

        public class Procedure: IProcedure<Request,QueryProjectionDto>
        {
            public async Task<QueryProjectionDto> ExecuteAsync(Request request, IDbConnection connection)
            {
                return await connection.QuerySingleProcAsync<QueryProjectionDto>("[Shipment].[ProcShipmentGet]", new { request.ShipmentId });
            }
        }

        public class QueryProjectionDto
        {
            public int ShipmentId { get; set; }
            public string Code { get; set; }
        }
    }
}
