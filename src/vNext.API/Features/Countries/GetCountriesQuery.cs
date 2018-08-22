using MediatR;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;
using vNext.Core.Models;

namespace vNext.API.Features.Countries
{
    public class GetCountriesQuery
    {
        public class Request : AuthenticatedRequest, IRequest<Response> { }

        public class Response
        {
            public IEnumerable<CountryDto> Countries { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            private readonly IProcedure<Request, IEnumerable<Country>> _procedure;
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
                        Countries = (await connection.QueryProcAsync<Country>("[Comsense].[ProcCountryGetAll]"))
                        .Select(x => CountryDto.FromCountry(x)).ToList()
                    };
                }
            }
        }

        public class Procedure : IProcedure<Request, Country>
        {
            public async Task<Country> ExecuteAsync(Request request, IDbConnection connection)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
