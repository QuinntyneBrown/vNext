using MediatR;
using System.Data;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.CompanyService.CompanyAddresses
{
    public class GetCompanyAddressesQuery
    {
        public class Request : AuthenticatedRequest, IRequest<Response> { }

        public class Response
        {
            public IEnumerable<CompanyAddressDto> CompanyAddresses { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            private readonly IProcedure<Request, IEnumerable<CompanyAddressDto>> _procedure;
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
                        CompanyAddresses = await connection.QueryProcAsync<CompanyAddressDto>("[Common].[ProcCompanyAddressGetAll]")
                    };
                }
            }
        }

        public class Procedure : IProcedure<Request, CompanyAddressDto>
        {
            public async Task<CompanyAddressDto> ExecuteAsync(Request request, IDbConnection connection)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
