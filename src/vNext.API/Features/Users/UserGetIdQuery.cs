using Dapper;
using MediatR;
using System.Data;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Users
{
    public class UserGetIdQuery
    {
        public class Request : AuthenticatedRequest, IRequest<Response> {
            public string Code { get; set; }
        }

        public class Response
        {
            public int UserId { get; set; }
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
                    return new Response()
                    {
                        UserId = await Procedure.ExecuteAsync(request,connection)
                    };
                }
            }
        }

        public class Procedure
        {
            public static async Task<short> ExecuteAsync(Request request, IDbConnection connection)
            {
                var dynamicParameters = new DynamicParameters();

                dynamicParameters.AddDynamicParams(new { request.Code });

                dynamicParameters.Add("UserId", SqlDbType.Int, direction: ParameterDirection.Output);

                await connection.ExecuteProcAsync("[Common].[ProcUserGetId]", dynamicParameters);

                return dynamicParameters.Get<short>("@UserId");
            }
        }
    }
}
