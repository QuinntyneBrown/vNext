using MediatR;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.AddressPhones
{
    public class AddressPhoneGetByAddressIdQuery
    {
        public class Request : AuthenticatedRequest, IRequest<Response> {
            public int AddressId { get; set; }
        }

        public class Response
        {
            public IEnumerable<AddressPhoneDto> AddressPhones { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            private readonly IProcedure<Request, IEnumerable<AddressPhoneDto>> _procedure;

            public Handler(IDbConnectionManager dbConnectionManager, IProcedure<Request, IEnumerable<AddressPhoneDto>> procedure)
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
                        AddressPhones = await _procedure.ExecuteAsync(request, connection)
                    };
                }
            }
        }

        public class Procedure: IProcedure<Request, IEnumerable<AddressPhoneDto>>
        {
            public async Task<IEnumerable<AddressPhoneDto>> ExecuteAsync(Request request, IDbConnection connection)
            {
                return await connection.QueryProcAsync<AddressPhoneDto>("[Comsense].[ProcAddressPhoneGetByAddressId]", new {
                    request.AddressId
                });
            }
        }
    }
}
