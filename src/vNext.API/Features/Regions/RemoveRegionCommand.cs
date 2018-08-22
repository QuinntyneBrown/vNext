using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Regions
{
    public class RemoveRegionCommand
    {
        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response>
        {
            public int RegionId { get; set; }
            public int ConcurrencyVersion { get; set; }
        }

        public class Response
        {
            public int Result { get; set; }
        }

        public class Handler : IRequestHandler<Request,Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            private readonly IProcedure<Request, short> _procedure;
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
                        Result = await SaveEntityCommandHandler.Handle(
                            request,
                            Procedure.ExecuteAsync,
                            "Region",
                            request.RegionId,
                            request.ConcurrencyVersion,
                            connection)
                    };
                }
            }
        }

        public class Procedure
        {
            public static async Task<short> ExecuteAsync(Request request, IDbConnection connection)
            {
                var result = await connection.ExecuteProcAsync("[Common].[ProcRegionDelete]", new { request.RegionId });

                return  (short)result;
            }
        }
    }
}
