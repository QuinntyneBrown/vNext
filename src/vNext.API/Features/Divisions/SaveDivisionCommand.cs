using Dapper;
using FluentValidation;
using MediatR;
using System.Data;
using System.Data;
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

        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response> {
            public DivisionDto Division { get; set; }
        }

        public class Response
        {			
            public int DivisionId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            public Handler( IDbConnectionManager dbConnectionManager)
                => _dbConnectionManager = dbConnectionManager;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    return new Response()
                    {
                        DivisionId = await Procedure.ExecuteAsync(request,connection)
                    };
                }
            }
        }

        public class Procedure
        {
            public static async Task<int> ExecuteAsync(Request request, IDbConnection connection)
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
