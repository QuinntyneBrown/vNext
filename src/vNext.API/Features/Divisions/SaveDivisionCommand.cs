using Dapper;
using FluentValidation;
using MediatR;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Divisions
{
    public class SaveDivisionCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.Division.DivisionId).NotNull();
            }
        }

        public class Request : IRequest<Response> {
            public DivisionDto Division { get; set; }
        }

        public class Response
        {			
            public int DivisionId { get; set; }
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
                        DivisionId = await Procedure.ExecuteAsync(request,connection)
                    };
                }
            }
        }

        public static class Procedure
        {
            public static async Task<int> ExecuteAsync(Request request, SqlConnection connection)
            {
                var dynamicParameters = new DynamicParameters();

                
                dynamicParameters.AddDynamicParams(new
                {
                    request.Division.DivisionId,
                    request.Division.Code
                });

                var parameterDirection = request.Division.DivisionId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                dynamicParameters.Add("DivisionId", request.Division.DivisionId, DbType.Int16, parameterDirection);

                await connection.ExecuteProcAsync("[Common].[ProcDivisionSave]", dynamicParameters);

                return dynamicParameters.Get<short>("@DivisionId");
            }
        }    
        
    }
}
