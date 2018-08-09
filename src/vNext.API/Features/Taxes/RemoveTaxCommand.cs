using MediatR;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Taxes
{
    public class RemoveTaxCommand
    {
        public class Request : IRequest<Response>
        {
            public TaxDto Tax { get; set; }
        }

        public class Response
        {
            public int TaxId { get; set; }
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
                        TaxId = await Procedure.ExecuteAsync(request, connection)
                    };
                }
            }
        }

        public static class Procedure
        {
            public static async Task<int> ExecuteAsync(Request request, SqlConnection connection)
            {
                return await connection.ExecuteProcAsync("[Comsense].[ProcTaxDelete]", new { request.Tax.TaxId });
            }
        }
    }
}
