using Dapper;
using FluentValidation;
using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.ContactService.ContactAddresses
{
    public class SaveContactAddressCommand
    {

        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.ContactAddress.ContactAddressId).NotNull();
            }
        }

        public class Request : AuthenticatedRequest, IRequest<Response> {
            public ContactAddressDto ContactAddress { get; set; }
        }

        public class Response
        {			
            public int ContactAddressId { get; set; }
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
                        request.ContactAddress.ContactAddressId,
                        request.ContactAddress.Code
                    });

                    var parameterDirection = request.ContactAddress.ContactAddressId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                    dynamicParameters.Add("ContactAddressId",  request.ContactAddress.ContactAddressId, DbType.Int16, parameterDirection);
                        
                    await connection.ExecuteProcAsync("[Common].[ProcContactAddressSave]", dynamicParameters);
                    
                    return new Response()
                    {
                        ContactAddressId = dynamicParameters.Get<short>("@ContactAddressId")
                    };
                }
            }
        }
    }
}
