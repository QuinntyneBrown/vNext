using MediatR;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.CompanyService.CompanyAddresses
{
    public class RemoveCompanyAddressCommand
    {
        public class Request : IRequest
        {
            public CompanyAddressDto CompanyAddress { get; set; }
        }

        public class Handler : IRequestHandler<Request>
        {
            private readonly ISqlConnectionManager _sqlConnectionManager;
            public Handler(ISqlConnectionManager sqlConnectionManager)
            {
                _sqlConnectionManager = sqlConnectionManager;
            }

            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _sqlConnectionManager.GetConnection())
                {
                    await connection.ExecuteProcAsync("[Common].[ProcCompanyAddressDelete]", new { request.CompanyAddress.CompanyAddressId });
                }
            }

        }
    }
}
