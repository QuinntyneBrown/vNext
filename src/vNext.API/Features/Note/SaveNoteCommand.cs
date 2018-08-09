using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Data;
using Dapper;
using vNext.Infrastructure.Data;
using vNext.Core.Interfaces; using vNext.Core.Extensions;
using FluentValidation;
using System.Data.SqlClient;

namespace vNext.API.Features.Notes
{
    public class SaveNoteCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.Note.NoteId).NotNull();
                RuleFor(request => request.Note.Note).NotNull();
            }
        }

        public class Request : IRequest<Response> {
            public NoteDto Note { get; set; }
        }

        public class Response
        {			
            public int NoteId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISqlConnectionManager _sqlConnectionManager;
            public Handler( ISqlConnectionManager sqlConnectionManager)
                => _sqlConnectionManager = sqlConnectionManager;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _sqlConnectionManager.GetConnection())
                {                    
                    return new Response()
                    {
                        NoteId = await new Prodcedure().ExecuteAsync(request.Note.NoteId, request.Note.Note, connection)
                    };
                }
            }
        }

        public class Prodcedure
        {
            public async Task<int> ExecuteAsync(int noteId, string note, SqlConnection connection)
            {
                var dynamicParameters = new DynamicParameters();

                dynamicParameters.AddDynamicParams(new { noteId, note });

                var parameterDirection = noteId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                dynamicParameters.Add("NoteId", noteId, DbType.Int16, parameterDirection);

                await connection.ExecuteProcAsync("[Comsense].[ProcNoteSave]", dynamicParameters);

                return dynamicParameters.Get<short>("@NoteId");
            }
        }
    }
}
