using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.ContactService.ContactAddresses
{
    public class RemoveContactAddressCommand
    {
        public class Request : AuthenticatedRequest, IRequest
        {
            public ContactAddressDto ContactAddress { get; set; }
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
                    await connection.ExecuteProcAsync("[Common].[ProcContactAddressDelete]", new { request.ContactAddress.ContactAddressId });
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
