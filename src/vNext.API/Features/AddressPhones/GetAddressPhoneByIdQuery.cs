using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.AddressPhones
{
    public class GetAddressPhoneByIdQuery
    {
        public class Request : AuthenticatedRequest, IRequest<Response>
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
            {
                _dbConnectionManager = dbConnectionManager;
            }

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

        public class Procedure : IProcedure<Request, AddressPhoneDto>
        {
            public async Task<AddressPhoneDto> ExecuteAsync(Request request, IDbConnection connection)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
