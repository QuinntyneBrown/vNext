using MediatR;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.API.Features.AddressEmails;
using vNext.API.Features.AddressPhones;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Addresses
{
    public class GetAddressByIdQuery
    {
        public class Request : AuthenticatedRequest, IRequest<Response>
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
            private readonly IProcedure<Request, AddressDto> _procedure;
            public Handler(IDbConnectionManager dbConnectionManager, IProcedure<Request, AddressDto> procedure)
            {
                _dbConnectionManager = dbConnectionManager;
                _procedure = procedure;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    return new Response() {
                        Address = await _procedure.ExecuteAsync(request, connection)
                    };
                }
            }           
        }
        
        public class Procedure: IProcedure<Request,AddressDto>
        {
            IProcedure<AddressEmailGetByAddressIdQuery.Request, IEnumerable<AddressEmailDto>> _addressEmailGetByAddressIdProcedure;
            IProcedure<AddressPhoneGetByAddressIdQuery.Request, IEnumerable<AddressPhoneDto>> _addressPhoneGetByAddressIdProcedure;
            public Procedure(
                IProcedure<AddressEmailGetByAddressIdQuery.Request, IEnumerable<AddressEmailDto>> addressEmailGetByAddressIdProcedure,
                IProcedure<AddressPhoneGetByAddressIdQuery.Request, IEnumerable<AddressPhoneDto>> addressPhoneGetByAddressIdProcedure
                )
            {
                _addressEmailGetByAddressIdProcedure = addressEmailGetByAddressIdProcedure;
                _addressPhoneGetByAddressIdProcedure = addressPhoneGetByAddressIdProcedure;
            }

            public async Task<AddressDto> ExecuteAsync(Request request, IDbConnection connection)
            {
                var address = await connection.QuerySingleProcAsync<AddressDto>("[Comsense].[ProcAddressGet]", new { request.AddressId });
                var addressBase = await connection.QuerySingleProcAsync<AddressDto>("[Comsense].[ProcAddressBaseGet]", new { address.AddressBaseId });
                address.CountrySubdivisionId = addressBase.CountrySubdivisionId;
                address.Address = addressBase.Address;
                address.City = addressBase.City;
                address.County = addressBase.County;
                address.PostalZipCode = addressBase.PostalZipCode;
                address.AddressPhones = await _addressPhoneGetByAddressIdProcedure.ExecuteAsync(new AddressPhoneGetByAddressIdQuery.Request() { AddressId = request.AddressId }, connection);                
                address.AddressEmails = await _addressEmailGetByAddressIdProcedure.ExecuteAsync(new AddressEmailGetByAddressIdQuery.Request() { AddressId = request.AddressId }, connection);
                return address;
            }
        }
    }
}
