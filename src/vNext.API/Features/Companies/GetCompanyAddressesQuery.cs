using vNext.Infrastructure.Data;
using Dapper;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using vNext.Core.Interfaces; using vNext.Core.Extensions;

namespace vNext.CompanyService.CompanyAddresses
{
    public class GetCompanyAddressesQuery
    {
        public class Request : IRequest<Response> { }

        public class Response
        {
            public IEnumerable<CompanyAddressDto> CompanyAddresses { get; set; }
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
                        CompanyAddresses = await connection.QueryProcAsync<CompanyAddressDto>("[Common].[ProcCompanyAddressGetAll]")
                    };
                }
            }
        }
    }
}
