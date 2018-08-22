using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.AddressEmailTypes
{
    public class GetAddressEmailTypeByIdQuery
    {
        public class Request : AuthenticatedRequest, IRequest<Response>
        {
            public int AddressEmailTypeId { get; set; }
        }

        public class Response
        {
            public AddressEmailTypeDto AddressEmailType { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
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
                        AddressEmailType = await connection.QuerySingleProcAsync<AddressEmailTypeDto>("[Comsense].[ProcAddressEmailTypeGet]", new { request.AddressEmailTypeId })
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
