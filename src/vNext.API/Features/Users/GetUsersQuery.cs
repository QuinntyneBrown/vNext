using Dapper;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Interfaces;
using vNext.Core.Models;

namespace vNext.API.Features.Users
{
    public class GetUsersQuery
    {
        public class Request : AuthenticatedRequest, IRequest<Response> { }

        public class Response
        {			
            public ICollection<UserDto> Users { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            public Handler(IDbConnectionManager dbConnectionManager)
                => _dbConnectionManager = dbConnectionManager;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using(var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    return new Response()
                    {
                        Users = (await connection.QueryAsync<User>("select * from [Common].[User]")).Select(x => UserDto.FromUser(x)).ToList()
                    };
                }
            }
        }
    }
}
