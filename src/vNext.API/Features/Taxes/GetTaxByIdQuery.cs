using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Taxes
{
    public class GetTaxByIdQuery
    {
        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response>
        {
            public int TaxId { get; set; }
        }

        public class Response
        {
            public TaxDto Tax { get; set; }
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
                        Tax = TaxDto.FromTax((await Procedure.ExecuteAsync(request, connection)))                        
                    };
                }
            }
        }

        public class Procedure
        {
            public static async Task<QueryProjectionDto> ExecuteAsync(Request request, IDbConnection connection)
            {
                return await connection.QuerySingleProcAsync<QueryProjectionDto>("[Comsense].[ProcTaxGet]", new { request.TaxId });
            }
        }

        public class QueryProjectionDto
        {
            public int TaxId { get; set; }
            public string Code { get; set; }
        }
    }
}
