using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Data;
using Dapper;
using vNext.Infrastructure.Data;
using vNext.Core.Interfaces; using vNext.Core.Extensions;
using FluentValidation;
using System.Data;

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

        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response> {
            public Request() { }
            public Request(string note) => Note = new NoteDto() { Note = note };
            public NoteDto Note { get; set; }
        }

        public class Response
        {			
            public int NoteId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            private readonly IProcedure<Request, short> _procedure;
            public Handler( IDbConnectionManager dbConnectionManager, IProcedure<Request, short> procedure)
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
                        NoteId = await _procedure.ExecuteAsync(request, connection)
                    };
                }
            }
        }

        public class Prodcedure: IProcedure<Request,short>
        {
            public async Task<short> ExecuteAsync(Request request, IDbConnection connection)
            {
                var dynamicParameters = new DynamicParameters();

                dynamicParameters.AddDynamicParams(new { request.Note.NoteId, request.Note.Note });

                var parameterDirection = request.Note.NoteId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                dynamicParameters.Add("NoteId", request.Note.NoteId, DbType.Int16, parameterDirection);

                await connection.ExecuteProcAsync("[Comsense].[ProcNoteSave]", dynamicParameters);

                return dynamicParameters.Get<short>("@NoteId");
            }
        }
    }
}
