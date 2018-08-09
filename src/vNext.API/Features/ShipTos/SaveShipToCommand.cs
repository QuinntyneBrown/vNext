using Dapper;
using FluentValidation;
using MediatR;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.ShipTos
{
    public class SaveShipToCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.ShipTo.ShipToId).NotNull();
            }
        }

        public class Request : IRequest<Response> {
            public ShipToDto ShipTo { get; set; }
        }

        public class Response
        {			
            public int ShipToId { get; set; }
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
                        ShipToId = await Procedure.ExecuteAsync(request,connection)
                    };
                }
            }
        }

        public static class Procedure
        {
            public static async Task<short> ExecuteAsync(Request request, SqlConnection connection)
            {
                var dynamicParameters = new DynamicParameters();

                var parameterDirection = request.ShipTo.ShipToId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                dynamicParameters.AddDynamicParams(new
                {
                    request.ShipTo.ShipToId,
                    request.ShipTo.Code
                });

                dynamicParameters.Add("ShipToId", request.ShipTo.ShipToId, DbType.Int16, parameterDirection);

                await connection.ExecuteProcAsync("[Common].[ProcShipToSave]", dynamicParameters);

                return dynamicParameters.Get<short>("@ShipToId");
            }
        }    
        
    }
}
