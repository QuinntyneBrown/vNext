using MediatR;
using System.Threading.Tasks;
using System.Threading;
using FluentValidation;
using Newtonsoft.Json;
using static System.Data.ParameterDirection;
using static System.Data.DbType;
using vNext.Core.Interfaces;
using Dapper;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Addresses
{
    public class SaveAddressCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.Address.AddressId).NotNull();
            }
        }

        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response> {
            public AddressDto Address { get; set; }
        }

        public class Response
        {			
            public int AddressId { get; set; }
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
                        request.Address.AddressId,
                        request.Address.Address,
                        request.Address.City,
                        request.Address.PostalZipCode,
                        request.Address.County,
                        request.Address.CountrySubDivisionId,
                        request.Address.Phone,
                        request.Address.Fax,
                        request.Address.Email,
                        request.Address.Website,
                        AddressPhones = JsonConvert.SerializeObject(request.Address.AddressPhones),
                        AddressEmails = JsonConvert.SerializeObject(request.Address.AddressEmails)
                    });

                    dynamicParameters.Add("AddressId", request.Address.AddressId, Int16, request.Address.AddressId == 0 ? Output : InputOutput);

                    await connection.ExecuteProcAsync("[Comsense].[ProcAddressSave]", dynamicParameters);

                    return new Response() {
                        AddressId = dynamicParameters.Get<short>("@AddressId")
                    };
                }
            }
        }
    }
}
