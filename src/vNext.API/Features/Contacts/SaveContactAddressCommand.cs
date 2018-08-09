using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Data;
using Dapper;
using vNext.Core.Interfaces;
using vNext.Core.Extensions;
using FluentValidation;
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

        public class Request : IRequest<Response> {
            public ContactAddressDto ContactAddress { get; set; }
        }

        public class Response
        {			
            public int ContactAddressId { get; set; }
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
