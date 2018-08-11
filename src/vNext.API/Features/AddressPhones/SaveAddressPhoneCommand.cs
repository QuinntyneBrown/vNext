using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Data;
using Dapper;
using vNext.Core.Interfaces;
using vNext.Core.Extensions;
using FluentValidation;
using vNext.Core.Interfaces;

namespace vNext.API.Features.AddressPhones
{
    public class SaveAddressPhoneCommand
    {

        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.AddressPhone.AddressPhoneId).NotNull();
            }
        }

        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response> {
            public AddressPhoneDto AddressPhone { get; set; }
        }

        public class Response
        {			
            public int AddressPhoneId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            public Handler(IDbConnectionManager dbConnectionManager)
            {
                _dbConnectionManager = dbConnectionManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.AddDynamicParams(new
                    {
                        request.AddressPhone.AddressPhoneId,
                        request.AddressPhone.Phone
                    });

                    var parameterDirection = request.AddressPhone.AddressPhoneId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                    dynamicParameters.Add("AddressPhoneId",  request.AddressPhone.AddressPhoneId, DbType.Int16, parameterDirection);
                        
                    await connection.ExecuteProcAsync("[Comsense].[ProcAddressPhoneSave]", dynamicParameters);
                    
                    return new Response()
                    {
                        AddressPhoneId = dynamicParameters.Get<short>("@AddressPhoneId")
                    };
                }
            }
        }
    }
}
