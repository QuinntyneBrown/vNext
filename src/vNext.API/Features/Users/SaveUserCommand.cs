using Dapper;
using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.API.Features.Notes;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Users
{
    public class SaveUserCommand
    {
        public class Request : AuthenticatedRequest, IRequest<Response>
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
            private readonly IProcedure<Request, short> _procedure;

            public Handler(IDbConnectionManager dbConnectionManager, IProcedure<Request, short> procedure)
            {
                _dbConnectionManager = dbConnectionManager;
                _procedure = procedure;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {                    
                    return new Response()
                    {
                        UserId = await _procedure.ExecuteAsync(request,connection)
                    };
                }
            }
        }

        public class Procedure: IProcedure<Request, short>
        {
            private readonly IProcedure<SaveNoteCommand.Request, short> _saveNoteProcedure;
            public Procedure(IProcedure<SaveNoteCommand.Request, short> saveNoteProcedure)
            {
                _saveNoteProcedure = saveNoteProcedure;
            }
            public async Task<short> ExecuteAsync(Request request, IDbConnection connection)
            {
                var dynamicParameters = new DynamicParameters();

                var noteId = await _saveNoteProcedure.ExecuteAsync(new SaveNoteCommand.Request(request.User.Note.Note), connection);
                
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
                        request.CurrentUserId
                    });
                
                var parameterDirection = request.User.UserId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                dynamicParameters.Add("UserId", request.User.UserId, DbType.Int16, parameterDirection);

                await connection.ExecuteProcAsync("[Common].[ProcUserSave]", dynamicParameters);

                return dynamicParameters.Get<short>("@UserId");
            }
        }
    }
}
