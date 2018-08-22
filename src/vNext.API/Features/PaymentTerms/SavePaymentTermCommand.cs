using Dapper;
using FluentValidation;
using MediatR;
using System.Data;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.PaymentTerms
{
    public class SavePaymentTermCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.PaymentTerm.PaymentTermId).NotNull();
            }
        }

        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response> {
            public PaymentTermDto PaymentTerm { get; set; }
        }

        public class Response
        {			
            public int PaymentTermId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
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
                        PaymentTermId = await Procedure.ExecuteAsync(request,connection)
                    };
                }
            }
        }

        public class Procedure
        {
            public static async Task<short> ExecuteAsync(Request request, IDbConnection connection)
            {
                var dynamicParameters = new DynamicParameters();

                var parameterDirection = request.PaymentTerm.PaymentTermId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                dynamicParameters.AddDynamicParams(new
                {
                    request.PaymentTerm.PaymentTermId,
                    request.PaymentTerm.Code
                });

                dynamicParameters.Add("PaymentTermId", request.PaymentTerm.PaymentTermId, DbType.Int16, parameterDirection);

                await connection.ExecuteProcAsync("[Common].[ProcPaymentTermSave]", dynamicParameters);

                return dynamicParameters.Get<short>("@PaymentTermId");
            }
        }    
        
    }
}
