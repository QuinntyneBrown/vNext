using Dapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Users
{
    public class SaveUserCommand
    {
        public class Request : IRequest<Response>
        {
            public UserDto User { get; set; }
        }

        public class Response
        {
            public int UserId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISqlConnectionManager _sqlConnectionManager;
            private readonly IHttpContextAccessor _httpContextAccessor;
            public Handler(IHttpContextAccessor httpContextAccessor, ISqlConnectionManager sqlConnectionManager)
            {
                _sqlConnectionManager = sqlConnectionManager;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _sqlConnectionManager.GetConnection())
                {
                    var code = _httpContextAccessor.HttpContext.User.Identity.Name;

                    return new Response()
                    {
                        UserId = await Procedure.ExecuteAsync(request,connection,code)
                    };
                }
            }
        }

        public static class Procedure
        {
            public static async Task<short> ExecuteAsync(Request request, SqlConnection connection, string createdByUserCode = null)
            {
                var dynamicParameters = new DynamicParameters();

                var noteId = await new Notes.SaveNoteCommand.Prodcedure().ExecuteAsync(0, request.User.Note.Note, connection);
                
                dynamicParameters.AddDynamicParams(new
                {
                    request.User.UserId,
                    request.User.Code,
                    request.User.Status,
                    request.User.CreatedByUserId,
                    request.User.CreatedDateTime,
                    request.User.ContactId,
                    request.User.DivisionId,
                    request.User.WarehouseId,
                    request.User.Settings,
                    noteId
                });

                if (request.User.UserId == default(int))
                {                    
                    dynamicParameters.AddDynamicParams(new
                    {
                        CreatedDateTime = System.DateTime.UtcNow,
                        UserId = await UserGetIdQuery.Procedure.ExecuteAsync(new UserGetIdQuery.Request() {
                            Code = createdByUserCode
                        }, connection)
                });
                }

                var parameterDirection = request.User.UserId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                dynamicParameters.Add("UserId", request.User.UserId, DbType.Int16, parameterDirection);

                await connection.ExecuteProcAsync("[Common].[ProcUserSave]", dynamicParameters);

                return dynamicParameters.Get<short>("@UserId");
            }
        }
    }
}
