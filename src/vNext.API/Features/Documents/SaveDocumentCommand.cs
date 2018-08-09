using Dapper;
using FluentValidation;
using MediatR;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace VNext.API.Features.Documents
{
    public class SaveDocumentCommand
    {

        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.Document.DocumentId).NotNull();
                RuleFor(request => request.Document.Document).NotNull();
            }
        }

        public class Request : IRequest<Response> {
            public DocumentDto Document { get; set; }
        }

        public class Response
        {
            public Response(short documentId) => DocumentId = documentId;

            public int DocumentId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISqlConnectionManager _sqlConnectionManager;
            public Handler( ISqlConnectionManager sqlConnectionManager)
                => _sqlConnectionManager = sqlConnectionManager;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var result = default(short);

                    using (var connection = _sqlConnectionManager.GetConnection())
                    {
                        connection.Open();

                        result = await new Procedure().ExecuteAsync(request, connection);
                    }

                    transaction.Complete();

                    return new Response(result);
                }
            }
        }

        public class Procedure
        {
            public async Task<short> ExecuteAsync(Request request, SqlConnection connection)
            {
                var dynamicParameters = new DynamicParameters();

                dynamicParameters.AddDynamicParams(new
                {
                    request.Document.DocumentId,
                    request.Document.Document,
                });

                var parameterDirection = request.Document.DocumentId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                dynamicParameters.Add("DocumentId", request.Document.DocumentId, DbType.Int16, parameterDirection);

                await connection.ExecuteProcAsync("[Comsense].[ProcDocumentSave]", dynamicParameters);

                return dynamicParameters.Get<short>("@DocumentId");
            }
        }
    }
}
