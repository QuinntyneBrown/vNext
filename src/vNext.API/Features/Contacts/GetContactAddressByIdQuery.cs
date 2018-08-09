using vNext.Core.Interfaces;
using Dapper;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Data;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.ContactService.ContactAddresses
{
    public class GetContactAddressByIdQuery
    {
        public class Request : IRequest<Response>
        {
            public int ContactAddressId { get; set; }
        }

        public class Response
        {
            public ContactAddressDto ContactAddress { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISqlConnectionManager _sqlConnectionManager;
            public Handler(ISqlConnectionManager sqlConnectionManager)
            {
                _sqlConnectionManager = sqlConnectionManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _sqlConnectionManager.GetConnection())
                {
                    return new Response()
                    {
                        ContactAddress = await connection.QuerySingleProcAsync<ContactAddressDto>("[Common].[ProcContactAddressGet]", new { request.ContactAddressId })
                    };
                }
            }
        }
    }
}
