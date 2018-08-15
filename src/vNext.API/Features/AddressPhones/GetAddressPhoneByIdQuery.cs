using vNext.Core.Interfaces;
using Dapper;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Data;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.AddressPhones
{
    public class GetAddressPhoneByIdQuery
    {
        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response>
        {
            public int AddressPhoneId { get; set; }
        }

        public class Response
        {
            public AddressPhoneDto AddressPhone { get; set; }
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
                    return new Response()
                    {
                        AddressPhone = await connection.QuerySingleProcAsync<AddressPhoneDto>("[Comsense].[ProcAddressPhoneGet]", new { request.AddressPhoneId })
                    };
                }
            }
        }
    }
}
