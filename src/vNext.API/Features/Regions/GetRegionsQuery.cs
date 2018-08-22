using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Regions
{
    public class GetRegionsQuery
    {
        public class Request : AuthenticatedRequest, IRequest<Response> { }

        public class Response
        {
            public IEnumerable<RegionDto> Regions { get; set; }
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
                    var result = (await Procedure.ExecuteAsync(request, connection));

                    return new Response()
                    {
                        Regions = result
                        .Select(x => RegionDto.FromRegion(x)) as IEnumerable<RegionDto>
                    };
                }
            }
        }

        public class Procedure
        {
            public static async Task<IEnumerable<QueryProjectionDto>> ExecuteAsync(Request request, IDbConnection connection)
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
