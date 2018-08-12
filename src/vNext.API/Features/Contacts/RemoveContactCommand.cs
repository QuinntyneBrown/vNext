using MediatR;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Contacts
{
    public class RemoveContactCommand
    {
        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response>
        {
            public int ContactId { get; set; }
            public int ConcurrencyVersion { get; set; }
        }

        public class Response
        {
            public int ContactId { get; set; }
        }

        public class Handler : IRequestHandler<Request,Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            public Handler(IDbConnectionManager dbConnectionManager)
                => _dbConnectionManager = dbConnectionManager;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    try
                    {
                        var result = await SaveEntityCommandHandler.Handle(request, (x, y) => Procedure.ExecuteAsync(x, y), "Contact", request.ContactId, request.ConcurrencyVersion, connection);

                        return new Response()
                        {
                            ContactId = result
                        };
                    }catch(Exception e)
                    {
                        throw e;
                    }
                }
            }

        }

        public static class Procedure
        {
            public static async Task<short> ExecuteAsync(Request request, IDbConnection connection)
            {
                return Convert.ToInt16(await connection.ExecuteProcAsync("[Common].[ProcContactDelete]", new { request.ContactId }));
            }
        }
    }
}
