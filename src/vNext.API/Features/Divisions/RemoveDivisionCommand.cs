using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Divisions
{
    public class RemoveDivisionCommand
    {
        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response>
        {
            public DivisionDto Division { get; set; }
        }

        public class Response
        {
            public int DivisionId { get; set; }
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
                        DivisionId = await Procedure.ExecuteAsync(request, connection)
                    };
                }
            }
        }

        public class Procedure
        {
            public static async Task<int> ExecuteAsync(Request request, IDbConnection connection)
            {
                return await connection.ExecuteProcAsync("[Common].[ProcDivisionDelete]", new { request.Division.DivisionId });
            }
        }
    }
}
