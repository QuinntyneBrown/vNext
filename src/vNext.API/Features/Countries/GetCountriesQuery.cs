using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;
using vNext.Core.Models;

namespace vNext.API.Features.Countries
{
    public class GetCountriesQuery
    {
        public class Request : IRequest<Response> { }

        public class Response
        {
            public IEnumerable<CountryDto> Countries { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISqlConnectionManager _sqlConnectionManager;
            public Handler(ISqlConnectionManager sqlConnectionManager)
                => _sqlConnectionManager = sqlConnectionManager;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _sqlConnectionManager.GetConnection())
                {
                    return new Response()
                    {
                        Countries = (await connection.QueryProcAsync<Country>("[Comsense].[ProcCountryGetAll]"))
                        .Select(x => CountryDto.FromCountry(x)).ToList()
                    };
                }
            }
        }
    }
}
