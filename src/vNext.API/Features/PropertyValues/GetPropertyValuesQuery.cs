using MediatR;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.PropertyValues
{
    public class GetPropertyValuesQuery
    {
        public class Request : IRequest<Response> { }

        public class Response
        {
            public IEnumerable<PropertyValueDto> PropertyValues { get; set; }
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
                    var result = await Procedure.ExecuteAsync(request, connection);

                    return new Response()
                    {
                        PropertyValues = result.Select(x => PropertyValueDto.FromPropertyValue(x))
                    };
                }
            }
        }

        public static class Procedure
        {
            public static async Task<IEnumerable<QueryProjectionDto>> ExecuteAsync(Request request, SqlConnection connection)
            {
                return await connection.QueryProcAsync<QueryProjectionDto>("[Comsense].[ProcPropertyValueGetAll]");
            }
        }

        public class QueryProjectionDto
        {
            public int PropertyValueId { get; set; }
            public string Code { get; set; }
        }
    }
}
