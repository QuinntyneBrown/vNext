using MediatR;
using System.Data;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.AddressEmailTypes
{
    public class GetAddressEmailTypesQuery
    {
        public class Request : AuthenticatedRequest, IRequest<Response> { }

        public class Response
        {
            public IEnumerable<AddressEmailTypeDto> AddressEmailTypes { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            private readonly IProcedure<Request, IEnumerable<AddressEmailTypeDto>> _procedure;
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
                        AddressEmailTypes = await connection.QueryProcAsync<AddressEmailTypeDto>("[Comsense].[ProcAddressEmailTypeGetAll]")
                    };
                }
            }
        }

        public class Procedure : IProcedure<Request, AddressEmailTypeDto>
        {
            public async Task<AddressEmailTypeDto> ExecuteAsync(Request request, IDbConnection connection)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
