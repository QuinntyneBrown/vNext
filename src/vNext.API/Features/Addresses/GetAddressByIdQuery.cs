using MediatR;
using System.Threading.Tasks;
using System.Threading;
using vNext.Core.Models;
using vNext.Core.Interfaces;
using vNext.Core.Extensions;
using System.Linq;
using vNext.Core.Interfaces;
using System.Data;

namespace vNext.API.Features.Addresses
{
    public class GetAddressByIdQuery
    {
        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response>
        {
            public int AddressId { get; set; }
        }

        public class Response
        {
            public AddressDto Address { get; set; }
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
                        Address = AddressDto.FromAddress(await Procedure.ExecuteAsync(request, connection))
                    };
                }
            }           
        }


        public static class Procedure
        {
            public static async Task<Address> ExecuteAsync(Request request, IDbConnection connection)
            {
                var address = await connection.QuerySingleProcAsync<Address>("[Comsense].[ProcAddressGet]", new { request.AddressId });
                var addressBase = await connection.QuerySingleProcAsync<AddressBase>("[Comsense].[ProcAddressBaseGet]", new { AddressBaseId = request.AddressId });
                address.CountrySubdivisionId = addressBase.CountrySubdivisionId;
                address.Address = addressBase.Address;
                address.City = addressBase.City;
                address.County = addressBase.County;
                address.PostalZipCode = addressBase.PostalZipCode;
                address.AddressPhones = (await connection.QueryProcAsync<AddressPhone>("[Comsense].[ProcAddressPhoneGetByAddressId]", new { request.AddressId })).ToList();
                address.AddressEmails = (await connection.QueryProcAsync<AddressEmail>("[Comsense].[ProcAddressEmailGetByAddressId]", new { request.AddressId })).ToList();
                return address;
            }
        }
    }
}
