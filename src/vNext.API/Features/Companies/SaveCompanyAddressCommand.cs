using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Data;
using Dapper;
using vNext.Infrastructure.Data;
using vNext.Core.Interfaces; using vNext.Core.Extensions;
using FluentValidation;

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

        public class Request : IRequest<Response> {
            public CompanyAddressDto CompanyAddress { get; set; }
        }

        public class Response
        {			
            public int CompanyAddressId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISqlConnectionManager _sqlConnectionManager;
            public Handler( ISqlConnectionManager sqlConnectionManager)
            {
                _sqlConnectionManager = sqlConnectionManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _sqlConnectionManager.GetConnection())
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
