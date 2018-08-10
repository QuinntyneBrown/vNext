using vNext.Core.Interfaces;
using Dapper;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.AddressEmails
{
    public class GetAddressEmailsQuery
    {
        public class Request : IRequest<Response> { }

        public class Response
        {
            public IEnumerable<AddressEmailDto> AddressEmails { get; set; }
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
                        AddressEmails = await connection.QueryProcAsync<AddressEmailDto>("[Comsense].[ProcAddressEmailGetAll]")
                    };
                }
            }
        }
    }
}
