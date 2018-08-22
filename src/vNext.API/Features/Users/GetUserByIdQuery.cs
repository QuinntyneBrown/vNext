using MediatR;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Users
{
    public class GetUserByIdQuery
    {
        public class Request : AuthenticatedRequest, IRequest<Response> {
            public int UserId { get; set; }
        }

        public class Response
        {			
            public UserDto User { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;

            public Handler(IDbConnectionManager dbConnectionManager)
            {
                _dbConnectionManager = dbConnectionManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    User = await _dbConnectionManager.GetConnection(request.CustomerKey).QuerySingleProcAsync<UserDto>("[Common].[ProcUserGet]", new { request.UserId })
                };
        }
    }
}
