using Dapper;
using FluentValidation;
using MediatR;
using System.Data;
using System.Data.SqlClient;
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

        public class Request : IRequest<Response> {
            public WarehouseDto Warehouse { get; set; }
        }

        public class Response
        {			
            public int WarehouseId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISqlConnectionManager _sqlConnectionManager;
            public Handler( ISqlConnectionManager sqlConnectionManager)
                => _sqlConnectionManager = sqlConnectionManager;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _sqlConnectionManager.GetConnection())
                {                    
                    return new Response()
                    {
                        WarehouseId = await Procedure.ExecuteAsync(request,connection)
                    };
                }
            }
        }

        public static class Procedure
        {
            public static async Task<short> ExecuteAsync(Request request, SqlConnection connection)
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
