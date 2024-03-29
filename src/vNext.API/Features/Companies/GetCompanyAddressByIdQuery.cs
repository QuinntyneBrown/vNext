using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.CompanyService.CompanyAddresses
{
    public class GetCompanyAddressByIdQuery
    {
        public class Request : AuthenticatedRequest, IRequest<Response>
        {
            public int CompanyAddressId { get; set; }
        }

        public class Response
        {
            public CompanyAddressDto CompanyAddress { get; set; }
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
                        CompanyAddress = await connection.QuerySingleProcAsync<CompanyAddressDto>("[Common].[ProcCompanyAddressGet]", new { request.CompanyAddressId })
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
