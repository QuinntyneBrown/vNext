using Dapper;
using FluentValidation;
using MediatR;
using System.Data;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Documents
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

        public class Request : AuthenticatedRequest, IRequest<Response> {
            public DocumentDto Document { get; set; }
        }

        public class Response
        {
            public Response(short documentId) => DocumentId = documentId;

            public int DocumentId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            public Handler( IDbConnectionManager dbConnectionManager)
                => _dbConnectionManager = dbConnectionManager;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var result = default(short);

                    using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
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
            public async Task<short> ExecuteAsync(Request request, IDbConnection connection)
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
