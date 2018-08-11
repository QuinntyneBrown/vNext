using MediatR;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.PaymentTerms
{
    public class GetPaymentTermByIdQuery
    {
        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response>
        {
            public int PaymentTermId { get; set; }
        }

        public class Response
        {
            public PaymentTermDto PaymentTerm { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
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
                        PaymentTerm = PaymentTermDto.FromPaymentTerm((await Procedure.ExecuteAsync(request, connection)))                        
                    };
                }
            }
        }

        public static class Procedure
        {
            public static async Task<QueryProjectionDto> ExecuteAsync(Request request, System.Data.IDbConnection connection)
            {
                return await connection.QuerySingleProcAsync<QueryProjectionDto>("[Common].[ProcPaymentTermGet]", new { request.PaymentTermId });
            }
        }

        public class QueryProjectionDto
        {
            public int PaymentTermId { get; set; }
            public string Code { get; set; }
        }
    }
}
