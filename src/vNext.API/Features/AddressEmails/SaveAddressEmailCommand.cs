using Dapper;
using FluentValidation;
using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.AddressEmails
{
    public class SaveAddressEmailCommand
    {

        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.AddressEmail.AddressEmailId).NotNull();
            }
        }

        public class Request : IRequest<Response> {
            public AddressEmailDto AddressEmail { get; set; }
        }

        public class Response
        {			
            public int AddressEmailId { get; set; }
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
                        request.AddressEmail.AddressEmailId,
                        request.AddressEmail.Email
                    });

                    var parameterDirection = request.AddressEmail.AddressEmailId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                    dynamicParameters.Add("AddressEmailId",  request.AddressEmail.AddressEmailId, DbType.Int16, parameterDirection);
                        
                    await connection.ExecuteProcAsync("[Comsense].[ProcAddressEmailSave]", dynamicParameters);
                    
                    return new Response()
                    {
                        AddressEmailId = dynamicParameters.Get<short>("@AddressEmailId")
                    };
                }
            }
        }
    }
}
