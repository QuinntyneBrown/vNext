using MediatR;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.CountrySubdivisions
{
    public class RemoveCountrySubdivisionCommand
    {
        public class Request : AuthenticatedRequest, IRequest
        {
            public CountrySubdivisionDto CountrySubdivision { get; set; }
        }

        public class Handler : IRequestHandler<Request>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            public Handler(IDbConnectionManager dbConnectionManager)
                => _dbConnectionManager = dbConnectionManager;

            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    await connection.ExecuteProcAsync("[Comsense].[ProcCountrySubdivisionDelete]", new { request.CountrySubdivision.CountrySubdivisionId });
                }
            }

        }
    }
}
