using Dapper;
using FluentValidation;
using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Warehouses
{
    public class SaveWarehouseCommand
    {

        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.Warehouse.WarehouseId).NotNull();
            }
        }

        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response> {
            public WarehouseDto Warehouse { get; set; }
        }

        public class Response
        {			
            public int WarehouseId { get; set; }
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
                        WarehouseId = await _procedure.ExecuteAsync(request,connection)
                    };
                }
            }
        }

        public class Procedure: IProcedure<Request,short>
        {
            public async Task<short> ExecuteAsync(Request request, IDbConnection connection)
            {
                var dynamicParameters = new DynamicParameters();

                dynamicParameters.AddDynamicParams(new
                {
                    request.Warehouse.WarehouseId,
                    request.Warehouse.Code
                });

                var parameterDirection = request.Warehouse.WarehouseId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                dynamicParameters.Add("WarehouseId", request.Warehouse.WarehouseId, DbType.Int16, parameterDirection);

                await connection.ExecuteProcAsync("[Product].[ProcWarehouseSave]", dynamicParameters);

                return dynamicParameters.Get<short>("@WarehouseId");
            }
        }
    }
}
