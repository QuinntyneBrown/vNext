using MediatR;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Users
{
    public class GetUserByIdQuery
    {
        public class Request : IRequest<Response> {
            public int UserId { get; set; }
        }

        public class Response
        {			
            public UserDto User { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISqlConnectionManager _sqlConnectionManager;
            public Handler(ISqlConnectionManager sqlConnectionManager)
            {
                _sqlConnectionManager = sqlConnectionManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    User = await _sqlConnectionManager.GetConnection().QuerySingleProcAsync<UserDto>("[Common].[ProcUserGet]", new { request.UserId })
                };
        }
    }
}
