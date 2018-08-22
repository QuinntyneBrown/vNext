using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.AddressEmails
{
    public class RemoveAddressEmailCommand
    {
        public class Request : AuthenticatedRequest, IRequest
        {
            public AddressEmailDto AddressEmail { get; set; }
        }

        public class Handler : IRequestHandler<Request>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            private readonly IProcedure<Request, int> _procedure;
            
            public Handler(IDbConnectionManager dbConnectionManager, IProcedure<Request, int> procedure)
            {
                _dbConnectionManager = dbConnectionManager;
                _procedure = procedure;
            } 

            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    await _procedure.ExecuteAsync(request, connection);
                }
            }
        }

        public class Procedure : IProcedure<Request, int>
        {
            public async Task<int> ExecuteAsync(Request request, IDbConnection connection)
            {
                return await connection.ExecuteProcAsync("[Comsense].[ProcAddressEmailDelete]", new { request.AddressEmail.AddressEmailId });
            }
        }
    }
}
