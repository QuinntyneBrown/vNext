using MediatR;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.CountrySubDivisions
{
    public class RemoveCountrySubDivisionCommand
    {
        public class Request : IRequest
        {
            public CountrySubDivisionDto CountrySubDivision { get; set; }
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
                    await connection.ExecuteProcAsync("[Comsense].[ProcCountrySubDivisionDelete]", new { request.CountrySubDivision.CountrySubDivisionId });
                }
            }

        }
    }
}
