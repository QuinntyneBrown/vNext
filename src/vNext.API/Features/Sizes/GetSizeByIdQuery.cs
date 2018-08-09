using MediatR;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Sizes
{
    public class GetSizeByIdQuery
    {
        public class Request : IRequest<Response>
        {
            public int SizeId { get; set; }
        }

        public class Response
        {
            public SizeDto Size { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
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
                        Size = SizeDto.FromSize((await Procedure.ExecuteAsync(request, connection)))                        
                    };
                }
            }
        }

        public static class Procedure
        {
            public static async Task<QueryProjectionDto> ExecuteAsync(Request request, SqlConnection connection)
            {
                return await connection.QuerySingleProcAsync<QueryProjectionDto>("[Comsense].[ProcSizeGet]", new { request.SizeId });
            }
        }

        public class QueryProjectionDto
        {
            public int SizeId { get; set; }
            public string Code { get; set; }
        }
    }
}
