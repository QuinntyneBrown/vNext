using MediatR;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.PaymentTerms
{
    public class GetPaymentTermsQuery
    {
        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response> { }

        public class Response
        {
            public IEnumerable<PaymentTermDto> PaymentTerms { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            private readonly IProcedure<Request, IEnumerable<QueryProjectionDto>> _procedure;
            public Handler(IDbConnectionManager dbConnectionManager)
            {
                _dbConnectionManager = dbConnectionManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    var result = await Procedure.ExecuteAsync(request, connection);

                    return new Response()
                    {
                        PaymentTerms = result.Select(x => PaymentTermDto.FromPaymentTerm(x))
                    };
                }
            }
        }

        public class Procedure
        {
            public static async Task<IEnumerable<QueryProjectionDto>> ExecuteAsync(Request request, IDbConnection connection)
            {
                return await connection.QueryProcAsync<QueryProjectionDto>("[Common].[ProcPaymentTermGetAll]");
            }
        }

        public class QueryProjectionDto
        {
            public int PaymentTermId { get; set; }
            public string Code { get; set; }
        }
    }
}
