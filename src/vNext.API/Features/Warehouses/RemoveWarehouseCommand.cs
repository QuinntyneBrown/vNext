using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Warehouses
{
    public class RemoveWarehouseCommand
    {
        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response>
        {
            public int WarehouseId { get; set; }
        }

        public class Response
        {
            public int Result { get; set; }
        }

        public class Handler : IRequestHandler<Request,Response>
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
                        Result = await Procedure.ExecuteAsync(request,connection)
                    };
                }
            }
        }

        public static class Procedure
        {
            public static async Task<short> ExecuteAsync(Request request, IDbConnection connection)
            {
                return (short)(await connection.ExecuteProcAsync("[Product].[ProcWarehouseDelete]", new { request.WarehouseId }));
            }
        }
    }
}
