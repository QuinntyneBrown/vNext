using MediatR;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Regions
{
    public class GetRegionsQuery
    {
        public class Request : IRequest<Response> { }

        public class Response
        {
            public IEnumerable<RegionDto> Regions { get; set; }
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
                        Regions = (await Procedure.ExecuteAsync(request, connection))
                        .Select(x => RegionDto.FromRegion(x)) as IEnumerable<RegionDto>
                    };
                }
            }
        }

        public static class Procedure
        {
            public static async Task<IEnumerable<QueryProjectionDto>> ExecuteAsync(Request request, SqlConnection connection)
                => await connection.QueryProcAsync<QueryProjectionDto>("[Common].[ProcRegionGetAll]");
        }

        public class QueryProjectionDto
        {
            public int RegionId { get; set; }
            public string Code { get; set; }
            public string Description { get; set; }
            public DateTime CreatedDateTime { get; set; }
            public int CreatedByUserId { get; set; }
            public int Sort { get; set; }
            public int ConcurrencyVersion { get; set; }
            public int NoteId { get; set; }
            public string Note { get; set; }
        }
    }
}
