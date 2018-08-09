using vNext.Core.Interfaces;
using Dapper;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Data;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.AddressEmailTypes
{
    public class GetAddressEmailTypeByIdQuery
    {
        public class Request : IRequest<Response>
        {
            public int AddressEmailTypeId { get; set; }
        }

        public class Response
        {
            public AddressEmailTypeDto AddressEmailType { get; set; }
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
                        AddressEmailType = await connection.QuerySingleProcAsync<AddressEmailTypeDto>("[Comsense].[ProcAddressEmailTypeGet]", new { request.AddressEmailTypeId })
                    };
                }
            }
        }
    }
}
