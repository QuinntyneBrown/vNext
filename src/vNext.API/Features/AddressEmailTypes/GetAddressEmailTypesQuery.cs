using vNext.Core.Interfaces;
using Dapper;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.AddressEmailTypes
{
    public class GetAddressEmailTypesQuery
    {
        public class Request : IRequest<Response> { }

        public class Response
        {
            public IEnumerable<AddressEmailTypeDto> AddressEmailTypes { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISqlConnectionManager _sqlConnectionManager;
            public Handler(ISqlConnectionManager sqlConnectionManager)
            {
                _sqlConnectionManager = sqlConnectionManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _sqlConnectionManager.GetConnection())
                {
                    return new Response()
                    {
                        AddressEmailTypes = await connection.QueryProcAsync<AddressEmailTypeDto>("[Comsense].[ProcAddressEmailTypeGetAll]")
                    };
                }
            }
        }
    }
}
