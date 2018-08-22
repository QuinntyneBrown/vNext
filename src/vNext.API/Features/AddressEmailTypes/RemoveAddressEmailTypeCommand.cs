using MediatR;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.AddressEmailTypes
{
    public class RemoveAddressEmailTypeCommand
    {
        public class Request : AuthenticatedRequest, IRequest
        {
            public AddressEmailTypeDto AddressEmailType { get; set; }
        }

        public class Handler : IRequestHandler<Request>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            public Handler(IDbConnectionManager dbConnectionManager)
            {
                _dbConnectionManager = dbConnectionManager;
            }

            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    await connection.ExecuteProcAsync("[Comsense].[ProcAddressEmailTypeDelete]", new { request.AddressEmailType.AddressEmailTypeId });
                }
            }

        }

        public class Procedure : IProcedure<Request, short>
        {
            public async Task<short> ExecuteAsync(Request request, IDbConnection connection)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
