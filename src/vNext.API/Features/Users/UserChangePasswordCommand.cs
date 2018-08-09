using Dapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Users
{
    public class UserChangePasswordCommand
    {
        public class Request :  IRequest { 
            public int UserId { get; set; }
            public string Password { get; set; }
        }
        
        public class Handler : IRequestHandler<Request>
        {
            private readonly ISqlConnectionManager _sqlConnectionManager;

            public Handler(ISqlConnectionManager sqlConnectionManager)
            {
                _sqlConnectionManager = sqlConnectionManager;
            }

            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _sqlConnectionManager.GetConnection())
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
    }
}
