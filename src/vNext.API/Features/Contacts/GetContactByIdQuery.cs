using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Contacts
{
    public class GetContactByIdQuery
    {
        public class Request : AuthenticatedRequest, IRequest<Response>
        {
            public int ContactId { get; set; }
        }

        public class Response
        {
            public ContactDto Contact { get; set; }
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
                        Contact = await connection.QuerySingleProcAsync<ContactDto>("[Common].[ProcContactGet]", new { request.ContactId })
                    };
                }
            }
        }

        public class Procedure : IProcedure<Request, ContactDto>
        {
            public async Task<ContactDto> ExecuteAsync(Request request, IDbConnection connection)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
