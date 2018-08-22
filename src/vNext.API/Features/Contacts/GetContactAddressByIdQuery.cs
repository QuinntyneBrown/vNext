using MediatR;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.ContactService.ContactAddresses
{
    public class GetContactAddressByIdQuery
    {
        public class Request : AuthenticatedRequest, IRequest<Response>
        {
            public int ContactAddressId { get; set; }
        }

        public class Response
        {
            public ContactAddressDto ContactAddress { get; set; }
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
                    return new Response()
                    {
                        ContactAddress = await connection.QuerySingleProcAsync<ContactAddressDto>("[Common].[ProcContactAddressGet]", new { request.ContactAddressId })
                    };
                }
            }
        }
    }
}
