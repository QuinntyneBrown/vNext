using Dapper;
using FluentValidation;
using MediatR;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Territories
{
    public class SaveTerritoryCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.Territory.TerritoryId).NotNull();
            }
        }

        public class Request : IRequest<Response> {
            public TerritoryDto Territory { get; set; }
        }

        public class Response
        {			
            public int TerritoryId { get; set; }
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
                        TerritoryId = await Procedure.ExecuteAsync(request,connection)
                    };
                }
            }
        }

        public static class Procedure
        {
            public static async Task<int> ExecuteAsync(Request request, SqlConnection connection)
            {
                var dynamicParameters = new DynamicParameters();

                var parameterDirection = request.Territory.TerritoryId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                dynamicParameters.AddDynamicParams(new
                {
                    request.Territory.TerritoryId,
                    request.Territory.Code
                });

                dynamicParameters.Add("TerritoryId", request.Territory.TerritoryId, DbType.Int16, parameterDirection);

                await connection.ExecuteProcAsync("[Common].[ProcTerritorySave]", dynamicParameters);

                return dynamicParameters.Get<short>("@TerritoryId");
            }
        }    
        
    }
}
