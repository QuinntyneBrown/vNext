using MediatR;
using System.Threading.Tasks;
using System.Threading;
using vNext.Core.Models;
using vNext.Core.Interfaces;
using vNext.Core.Extensions;
using System.Linq;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Addresses
{
    public class GetAddressByIdQuery
    {
        public class Request : IRequest<Response>
        {
            public int AddressId { get; set; }
        }

        public class Response
        {
            public AddressDto Address { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISqlConnectionManager _sqlConnectionManager;
            public Handler(ISqlConnectionManager sqlConnectionManager)
                => _sqlConnectionManager = sqlConnectionManager;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _sqlConnectionManager.GetConnection())
                {
                    var address = await connection.QuerySingleProcAsync<Address>("[Comsense].[ProcAddressGet]", new { request.AddressId});
                    address.AddressPhones = (await connection.QueryProcAsync<AddressPhone>("[Comsense].[ProcAddressPhoneGetByAddressId]", new { request.AddressId })).ToList();
                    address.AddressEmails = (await connection.QueryProcAsync<AddressEmail>("[Comsense].[ProcAddressEmailGetByAddressId]", new { request.AddressId })).ToList();
                    return new Response() {
                        Address = AddressDto.FromAddress(address)
                    };
                }
            }
        }
    }
}
