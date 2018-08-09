using MediatR;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.AddressEmails
{
    public class GetAddressEmailByIdQuery
    {
        public class Request : IRequest<Response>
        {
            public int AddressEmailId { get; set; }
        }

        public class Response
        {
            public AddressEmailDto AddressEmail { get; set; }
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
                    return new Response()
                    {
                        AddressEmail = await connection.QuerySingleProcAsync<AddressEmailDto>("[Comsense].[ProcAddressEmailGet]", new { request.AddressEmailId })
                    };
                }
            }
        }

        public static class Procedure
        {

        }
    }
}
