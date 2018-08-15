using MediatR;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Companies
{
    public class GetCompanyByIdQuery
    {
        public class Request : AuthenticatedRequest, IRequest<Response>
        {
            public int CompanyId { get; set; }
        }

        public class Response
        {
            public CompanyDto Company { get; set; }
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
                        Company = await connection.QuerySingleProcAsync<CompanyDto>("[Common].[ProcCompanyGet]", new { request.CompanyId })
                    };
                }
            }
        }
    }
}
