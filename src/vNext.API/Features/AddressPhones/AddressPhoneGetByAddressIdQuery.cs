using MediatR;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
            public Handler(IDbConnectionManager dbConnectionManager)
			    => _dbConnectionManager = dbConnectionManager;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    return new Response()
                    {
                        AddressPhones = await Procedure.ExecuteAsync(request, connection)
                    };
                }
            }
        }

        public class Procedure
        {
            public static async Task<IEnumerable<AddressPhoneDto>> ExecuteAsync(Request request, IDbConnection connection)
            {
                return await connection.QueryProcAsync<AddressPhoneDto>("[Comsense].[ProcAddressPhoneGetByAddressId]", new {
                    request.AddressId
                });
            }
        }
    }
}
