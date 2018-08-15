using Dapper;
using FluentValidation;
using MediatR;
using Newtonsoft.Json;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;
using static System.Data.DbType;
using static System.Data.ParameterDirection;

namespace vNext.API.Features.Addresses
{
    public class SaveAddressCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {

            }
        }

        public class Request : AuthenticatedRequest, IRequest<Response> {
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
                    return new Response() {
                        AddressId = await Procedure.ExecuteAsync(request,connection)
                    };
                }
            }
        }

        public class Procedure {

            public static async Task<short> ExecuteAsync(Request request, IDbConnection connection)
            {
                var dynamicParameters = new DynamicParameters();

                dynamicParameters.AddDynamicParams(new
                {
                    request.Address.AddressId,
                    request.Address.Address,
                    request.Address.City,
                    request.Address.PostalZipCode,
                    request.Address.County,
                    request.Address.CountrySubdivisionId,
                    request.Address.Phone,
                    request.Address.Fax,
                    request.Address.Email,
                    request.Address.Website,
                    AddressPhones = JsonConvert.SerializeObject(request.Address.AddressPhones),
                    AddressEmails = JsonConvert.SerializeObject(request.Address.AddressEmails)
                });

                dynamicParameters.Add("AddressId", request.Address.AddressId, Int16, request.Address.AddressId == 0 ? Output : InputOutput);

                await connection.ExecuteProcAsync("[Comsense].[ProcAddressSave]", dynamicParameters);

                return dynamicParameters.Get<short>("@AddressId");
            }
        }
    }
}
