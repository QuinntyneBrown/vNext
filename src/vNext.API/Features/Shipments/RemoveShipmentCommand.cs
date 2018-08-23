using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Shipments
{
    public class RemoveShipmentCommand
    {
        public class Request : AuthenticatedRequest, IRequest<Response>
        {
            public ShipmentDto Shipment { get; set; }
        }

        public class Response
        {
            public int ShipmentId { get; set; }
        }

        public class Handler : IRequestHandler<Request,Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            private readonly IProcedure<Request, int> _procedure;
            public Handler(IDbConnectionManager dbConnectionManager, IProcedure<Request, int> procedure)
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
                        ShipmentId = await _procedure.ExecuteAsync(request, connection)
                    };
                }
            }
        }

        public class Procedure: IProcedure<Request, int>
        {
            public async Task<int> ExecuteAsync(Request request, IDbConnection connection)
            {
                return await connection.ExecuteProcAsync("[Shipment].[ProcShipmentDelete]", new { request.Shipment.ShipmentId });
            }
        }
    }
}
