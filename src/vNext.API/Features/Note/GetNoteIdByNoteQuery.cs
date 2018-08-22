using Dapper;
using MediatR;
using System.Data;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Notes
{
    public class GetNoteIdByNoteQuery
    {
        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response> {
            public string Note { get; set; }
        }

        public class Response
        {
            public int NoteId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            public Handler(IDbConnectionManager dbConnectionManager)
            {
                _dbConnectionManager = dbConnectionManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    return new Response()
                    {
                        NoteId = await Procedure.ExecuteAsync(connection,request.Note)
                    };
                }
            }
        }

        public class Procedure {
            public static async Task<int> ExecuteAsync(IDbConnection connection, string note)
            {
                var dynamicParameters = new DynamicParameters();

                dynamicParameters.AddDynamicParams(new { note });

                dynamicParameters.Add("NoteId", dbType: DbType.Int16, direction: ParameterDirection.Output);

                await connection.ExecuteProcAsync("[Comsense].[ProcNoteGetId]", dynamicParameters);

                return dynamicParameters.Get<short>("@NoteId");
            }

        }
    }
}
