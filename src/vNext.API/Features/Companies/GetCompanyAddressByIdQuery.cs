using vNext.Infrastructure.Data;
using Dapper;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Data;
using vNext.Core.Interfaces; using vNext.Core.Extensions;

namespace vNext.CompanyService.CompanyAddresses
{
    public class GetCompanyAddressByIdQuery
    {
        public class Request : IRequest<Response>
        {
            public int CompanyAddressId { get; set; }
        }

        public class Response
        {
            public CompanyAddressDto CompanyAddress { get; set; }
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
                        CompanyAddress = await connection.QuerySingleProcAsync<CompanyAddressDto>("[Common].[ProcCompanyAddressGet]", new { request.CompanyAddressId })
                    };
                }
            }
        }
    }
}
