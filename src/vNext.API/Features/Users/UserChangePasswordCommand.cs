using Dapper;
using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Users
{
    public class UserChangePasswordCommand
    {
        public class Request : AuthenticatedRequest, IRequest { 
            public int UserId { get; set; }
            public string Password { get; set; }
        }
        
        public class Handler : IRequestHandler<Request>
        {
            private readonly IDbConnectionManager _dbConnectionManager;

            public Handler(IDbConnectionManager dbConnectionManager)
            {
                _dbConnectionManager = dbConnectionManager;
            }

            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.AddDynamicParams(new
                    {
                        request.UserId,
                        request.Password
                    });
                    
                    await connection.ExecuteProcAsync("[Common].[ProcUserChangePassword]", dynamicParameters);
                }
            }
        }

        public class Procedure : IProcedure<Request, short>
        {
            public async Task<short> ExecuteAsync(Request request, IDbConnection connection)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
