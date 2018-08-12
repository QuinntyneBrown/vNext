using Dapper;
using FluentValidation;
using MediatR;
using System.Data;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Taxes
{
    public class SaveTaxCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.Tax.TaxId).NotNull();
            }
        }

        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response> {
            public TaxDto Tax { get; set; }
        }

        public class Response
        {			
            public int TaxId { get; set; }
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
                        TaxId = await Procedure.ExecuteAsync(request,connection)
                    };
                }
            }
        }

        public static class Procedure
        {
            public static async Task<short> ExecuteAsync(Request request, IDbConnection connection)
            {
                var dynamicParameters = new DynamicParameters();

                var parameterDirection = request.Tax.TaxId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                dynamicParameters.AddDynamicParams(new
                {
                    request.Tax.TaxId,
                    request.Tax.Code
                });

                dynamicParameters.Add("TaxId", request.Tax.TaxId, DbType.Int16, parameterDirection);

                await connection.ExecuteProcAsync("[Common].[ProcTaxSave]", dynamicParameters);

                return dynamicParameters.Get<short>("@TaxId");
            }
        }    
        
    }
}
