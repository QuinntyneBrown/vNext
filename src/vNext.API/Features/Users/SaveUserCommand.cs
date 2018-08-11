using Dapper;
using MediatR;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Users
{
    public class SaveUserCommand
    {
        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response>
        {
            public UserDto User { get; set; }
        }

        public class Response
        {
            public int UserId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;

            public Handler(IDbConnectionManager dbConnectionManager)
                => _dbConnectionManager = dbConnectionManager;

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

        public static class Procedure
        {
            public static async Task<short> ExecuteAsync(Request request, System.Data.IDbConnection connection)
            {
                var dynamicParameters = new DynamicParameters();

                var noteId = await new Notes.SaveNoteCommand.Prodcedure().ExecuteAsync(new Notes.SaveNoteCommand.Request() {
                    Note = request.User.Note
                }, connection);
                
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
                    dynamicParameters.AddDynamicParams(new
                    {
                        request.CurrentDateTime,
                        request.UserId
                    });
                
                var parameterDirection = request.User.UserId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                dynamicParameters.Add("UserId", request.User.UserId, DbType.Int16, parameterDirection);

                await connection.ExecuteProcAsync("[Common].[ProcUserSave]", dynamicParameters);

                return dynamicParameters.Get<short>("@UserId");
            }
        }
    }
}
