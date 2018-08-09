using Dapper;
using FluentValidation;
using MediatR;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Sizes
{
    public class SaveSizeCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.Size.SizeId).NotNull();
            }
        }

        public class Request : IRequest<Response> {
            public SizeDto Size { get; set; }
        }

        public class Response
        {			
            public int SizeId { get; set; }
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
                        SizeId = await Procedure.ExecuteAsync(request,connection)
                    };
                }
            }
        }

        public static class Procedure
        {
            public static async Task<short> ExecuteAsync(Request request, SqlConnection connection)
            {
                var dynamicParameters = new DynamicParameters();

                var parameterDirection = request.Size.SizeId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                dynamicParameters.AddDynamicParams(new
                {
                    request.Size.SizeId,
                    request.Size.Code
                });

                dynamicParameters.Add("SizeId", request.Size.SizeId, DbType.Int16, parameterDirection);

                await connection.ExecuteProcAsync("[Common].[ProcSizeSave]", dynamicParameters);

                return dynamicParameters.Get<short>("@SizeId");
            }
        }    
        
    }
}
