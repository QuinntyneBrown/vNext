using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.PropertyValues
{
    public class GetPropertyValueByIdQuery
    {
        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response>
        {
            public int PropertyValueId { get; set; }
        }

        public class Response
        {
            public PropertyValueDto PropertyValue { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            private readonly IProcedure<Request, QueryProjectionDto> _procedure;
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
                        PropertyValue = PropertyValueDto.FromPropertyValue((await Procedure.ExecuteAsync(request, connection)))                        
                    };
                }
            }
        }

        public class Procedure
        {
            public static async Task<QueryProjectionDto> ExecuteAsync(Request request, IDbConnection connection)
            {
                return await connection.QuerySingleProcAsync<QueryProjectionDto>("[Comsense].[ProcPropertyValueGet]", new { request.PropertyValueId });
            }
        }

        public class QueryProjectionDto
        {
            public int PropertyValueId { get; set; }
            public string Code { get; set; }
        }
    }
}
