using Dapper;
using FluentValidation;
using MediatR;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.MostRecentlyUseds
{
    public class SaveMostRecentlyUsedCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.MostRecentlyUsed.MostRecentlyUsedId).NotNull();
            }
        }

        public class Request : IRequest<Response> {
            public MostRecentlyUsedDto MostRecentlyUsed { get; set; }
        }

        public class Response
        {			
            public int MostRecentlyUsedId { get; set; }
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
                        MostRecentlyUsedId = await Procedure.ExecuteAsync(request,connection)
                    };
                }
            }
        }

        public static class Procedure
        {
            public static async Task<short> ExecuteAsync(Request request, SqlConnection connection)
            {
                var dynamicParameters = new DynamicParameters();

                var parameterDirection = request.MostRecentlyUsed.MostRecentlyUsedId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                dynamicParameters.AddDynamicParams(new
                {
                    request.MostRecentlyUsed.MostRecentlyUsedId,
                    request.MostRecentlyUsed.Code
                });

                dynamicParameters.Add("MostRecentlyUsedId", request.MostRecentlyUsed.MostRecentlyUsedId, DbType.Int16, parameterDirection);

                await connection.ExecuteProcAsync("[Common].[ProcMostRecentlyUsedSave]", dynamicParameters);

                return dynamicParameters.Get<short>("@MostRecentlyUsedId");
            }
        }    
        
    }
}
