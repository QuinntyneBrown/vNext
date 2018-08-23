using Dapper;
using FluentValidation;
using MediatR;
using System.Data;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Shipments
{
    public class SaveShipmentCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.Shipment.ShipmentId).NotNull();
            }
        }

        public class Request : AuthenticatedRequest, IRequest<Response> {
            public ShipmentDto Shipment { get; set; }
        }

        public class Response
        {			
            public int ShipmentId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
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
                        ShipmentId = await _procedure.ExecuteAsync(request,connection)
                    };
                }
            }
        }

        public class Procedure: IProcedure<Request, short>
        {
            public async Task<short> ExecuteAsync(Request request, IDbConnection connection)
            {
                var dynamicParameters = new DynamicParameters();

                var parameterDirection = request.Shipment.ShipmentId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                dynamicParameters.AddDynamicParams(new
                {
                    request.Shipment.ShipmentId,
                    request.Shipment.Code
                });

                dynamicParameters.Add("ShipmentId", request.Shipment.ShipmentId, DbType.Int16, parameterDirection);

                await connection.ExecuteProcAsync("[Shipment].[ProcShipmentSave]", dynamicParameters);

                return dynamicParameters.Get<short>("@ShipmentId");
            }
        }    
        
    }
}
