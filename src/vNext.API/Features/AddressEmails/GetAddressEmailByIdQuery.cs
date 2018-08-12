using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.AddressEmails
{
    public class GetAddressEmailByIdQuery
    {
        public class Request : AuthenticatedRequest, IRequest<Response>
        {
            public int AddressEmailId { get; set; }
        }

        public class Response
        {
            public AddressEmailDto AddressEmail { get; set; }
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
                        AddressEmail = await Procedure.ExecuteAsync(request,connection)
                    };
                }
            }
        }

        public static class Procedure
        {
            public static async Task<AddressEmailDto> ExecuteAsync(Request request, IDbConnection connection)
            {
                return await connection.QuerySingleProcAsync<AddressEmailDto>("[Comsense].[ProcAddressEmailGet]", new { request.AddressEmailId });
            }
        }
    }
}
