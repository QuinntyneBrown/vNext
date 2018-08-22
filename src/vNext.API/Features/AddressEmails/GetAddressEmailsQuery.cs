using MediatR;
using System.Data;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.AddressEmails
{
    public class GetAddressEmailsQuery
    {
        public class Request : AuthenticatedRequest, IRequest<Response> { }

        public class Response
        {
            public IEnumerable<AddressEmailDto> AddressEmails { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            private readonly IProcedure<Request, IEnumerable<AddressEmailDto>> _procedure;

            public Handler(IDbConnectionManager dbConnectionManager, IProcedure<Request, IEnumerable<AddressEmailDto>> procedure)
            {
                _dbConnectionManager = dbConnectionManager;
                _procedure = procedure;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    return new Response()
                    {
                        AddressEmails = await _procedure.ExecuteAsync(request, connection)
                    };
                }
            }
        }

        public class Procedure : IProcedure<Request, IEnumerable<AddressEmailDto>>
        {
            public async Task<IEnumerable<AddressEmailDto>> ExecuteAsync(Request request, IDbConnection connection)
            {
                return await connection.QueryProcAsync<AddressEmailDto>("[Comsense].[ProcAddressEmailGetAll]");
            }
        }
    }
}
