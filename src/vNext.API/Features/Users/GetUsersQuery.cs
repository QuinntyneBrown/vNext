using Dapper;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Interfaces;
using vNext.Core.Models;

namespace vNext.API.Features.Users
{
    public class GetUsersQuery
    {
        public class Request : IRequest<Response> { }

        public class Response
        {			
            public ICollection<UserDto> Users { get; set; }
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
                    Users = (await _sqlConnectionManager.GetConnection()
                    .QueryAsync<User>("select * from [Common].[User]")).Select(x => UserDto.FromUser(x)).ToList()
                };
        }
    }
}
