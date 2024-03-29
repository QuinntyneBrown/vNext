using Dapper;
using FluentValidation;
using MediatR;
using System.Data;
using System.Data;
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

        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response> {
            public SizeDto Size { get; set; }
        }

        public class Response
        {			
            public int SizeId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            private readonly IProcedure<Request, short> _procedure;
            public Handler( IDbConnectionManager dbConnectionManager)
            {
                _dbConnectionManager = dbConnectionManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    return new Response()
                    {
                        SizeId = await Procedure.ExecuteAsync(request,connection)
                    };
                }
            }
        }

        public class Procedure
        {
            public static async Task<short> ExecuteAsync(Request request, IDbConnection connection)
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
