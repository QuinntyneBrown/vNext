using vNext.Infrastructure.Data;
using Dapper;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Data;
using vNext.Core.Interfaces; using vNext.Core.Extensions;

namespace vNext.API.Features.Companies
{
    public class GetCompanyByIdQuery
    {
        public class Request : IRequest<Response>
        {
            public int CompanyId { get; set; }
        }

        public class Response
        {
            public CompanyDto Company { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISqlConnectionManager _sqlConnectionManager;
            public Handler(ISqlConnectionManager sqlConnectionManager)
            {
                _sqlConnectionManager = sqlConnectionManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _sqlConnectionManager.GetConnection())
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
