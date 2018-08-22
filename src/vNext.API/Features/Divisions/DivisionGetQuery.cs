using MediatR;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Divisions
{
    public class DivisionGetQuery
    {
        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response> {
            public int DivisionId { get; set; }
        }

        public class Response
        {
            public IEnumerable<DivisionDto> Divisions { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            private readonly IProcedure<Request, IEnumerable<QueryProjectionDto>> _procedure;
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
                        Divisions = (await Procedure.ExecuteAsync(request, connection))
                        .Select(x => DivisionDto.FromDivision(x)) as IEnumerable<DivisionDto>
                    };
                }
            }
        }

        public class Procedure
        {
            public static async Task<IEnumerable<QueryProjectionDto>> ExecuteAsync(Request request, IDbConnection connection)
            {
                return await connection.QueryProcAsync<QueryProjectionDto>("[Common].[ProcDivisionGet]", new { request.DivisionId });
            }
        }

        public class QueryProjectionDto
        {
            public int DivisionId { get; set; }
            public string Code { get; set; }
            public string Description { get; set; }
            public int Status { get; set; }
            public int NoteId { get; set; }
            public string Note { get; set; }
            public int AddressId { get; set; }
            public int RegionId { get; set; }
            public int Sort { get; set; }
            public int ConcurrencyVersion { get; set; }
        }
    }
}
