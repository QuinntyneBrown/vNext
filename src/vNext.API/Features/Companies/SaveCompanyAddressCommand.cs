using Dapper;
using FluentValidation;
using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.CompanyService.CompanyAddresses
{
    public class SaveCompanyAddressCommand
    {

        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.CompanyAddress.CompanyAddressId).NotNull();
            }
        }

        public class Request : AuthenticatedRequest, IRequest<Response> {
            public CompanyAddressDto CompanyAddress { get; set; }
        }

        public class Response
        {			
            public int CompanyAddressId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            public Handler(IDbConnectionManager dbConnectionManager)
                => _dbConnectionManager = dbConnectionManager;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.AddDynamicParams(new
                    {
                        request.CompanyAddress.CompanyAddressId,
                        request.CompanyAddress.Code
                    });

                    var parameterDirection = request.CompanyAddress.CompanyAddressId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                    dynamicParameters.Add("CompanyAddressId",  request.CompanyAddress.CompanyAddressId, DbType.Int16, parameterDirection);
                        
                    await connection.ExecuteProcAsync("[Common].[ProcCompanyAddressSave]", dynamicParameters);
                    
                    return new Response()
                    {
                        CompanyAddressId = dynamicParameters.Get<short>("@CompanyAddressId")
                    };
                }
            }
        }
    }
}
