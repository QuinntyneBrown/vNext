using MediatR;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Sizes
{
    public class RemoveSizeCommand
    {
        public class Request : IRequest<Response>
        {
            public SizeDto Size { get; set; }
        }

        public class Response
        {
            public int SizeId { get; set; }
        }

        public class Handler : IRequestHandler<Request,Response>
        {
            private readonly ISqlConnectionManager _sqlConnectionManager;
            public Handler(ISqlConnectionManager sqlConnectionManager)
                => _sqlConnectionManager = sqlConnectionManager;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _sqlConnectionManager.GetConnection())
                {
                    return new Response()
                    {
                        SizeId = await Procedure.ExecuteAsync(request, connection)
                    };
                }
            }
        }

        public static class Procedure
        {
            public static async Task<int> ExecuteAsync(Request request, SqlConnection connection)
            {
                return await connection.ExecuteProcAsync("[Comsense].[ProcSizeDelete]", new { request.Size.SizeId });
            }
        }
    }
}
