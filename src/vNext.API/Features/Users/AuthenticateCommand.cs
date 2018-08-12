using Dapper;
using MediatR;
using System.Data;
using System.Threading.Tasks;
using System.Threading;
using vNext.Core.Interfaces;
using vNext.Core.Identity;
using vNext.Core.Extensions;
using System.Data;

namespace vNext.API.Features.Users
{
    public class AuthenticateCommand
    {
        public class Request : IRequest<Response>
        {
            public string Code { get; set; }
            public string Password { get; set; }
            public string CustomerKey { get; set; }
        }

        public class Response
        {
            public string AccessToken { get; set; }
            public int UserId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISecurityTokenFactory _securityTokenFactory;
            private readonly IDbConnectionManager _dbConnectionManager;
            public Handler(IDbConnectionManager dbConnectionManager, ISecurityTokenFactory securityTokenFactory)
            {
                _dbConnectionManager = dbConnectionManager;
                _securityTokenFactory = securityTokenFactory;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using(var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    var userId = await Procedure.ExecuteAsync(request, connection);
                    return new Response()
                    {
                        AccessToken = _securityTokenFactory.Create(request.Code, userId, request.CustomerKey),
                        UserId = userId
                    };
                }
            }
        }

        public static class Procedure
        {
            public static async Task<int> ExecuteAsync(Request request, IDbConnection connection)
            {
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("Code", request.Code);
                dynamicParameters.Add("Password", request.Password);
                dynamicParameters.Add("SelectResult", 1);
                dynamicParameters.Add("UserId", SqlDbType.Int, direction: ParameterDirection.Output);

                await connection.QueryProcAsync<int>("[Common].[ProcUserAuthenticate]", dynamicParameters);

                return dynamicParameters.Get<int>("UserId");
            }
        }
    }
}
