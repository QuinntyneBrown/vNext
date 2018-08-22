using Dapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Contacts
{
    public class ContactGetAllQuery
    {
        public class Request : AuthenticatedRequest, IRequest<Response> { }

        public class Response
        {
            public IEnumerable<ContactDto> Contacts { get; set; }
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
                    var queryResult = (await Procedure.ExecuteAsync(request, connection));

                    return new Response()
                    {
                        Contacts = queryResult
                        .Select(x => ContactDto.FromContact(x)) as IEnumerable<ContactDto>
                    };
                }
            }
        }

        public class Procedure
        {
            public static async Task<IEnumerable<QueryProjectionDto>> ExecuteAsync(Request request, IDbConnection connection)
            {
                return await connection.QueryAsync<QueryProjectionDto>(@"
                SELECT 
                    [Contact].[ContactId],
	                [Contact].[FirstName],
	                [Contact].[MiddleName],
	                [Contact].[LastName],
	                [Contact].[CompanyName],
	                [Contact].[CreatedDateTime],
	                [Contact].[CreatedDate],
	                [Contact].[CreatedByUserId],
	                [Contact].[AddressId],
	                [Contact].[DocumentId],
	                [Contact].[NoteId],
	                [Note].[Note],
	                ISNULL([Concurrency].[Version], 0) AS [ConcurrencyVersion]
                FROM
	                [Common].[Contact]
	                JOIN [Comsense].[Note] ON
		                [Contact].[NoteId] = [Note].[NoteId]
	                LEFT OUTER JOIN [Comsense].[Concurrency] ON
		                [Contact].[ContactId] = [Concurrency].[Id] AND
		                [Concurrency].[Domain] = N'Contact'");
            }
        }

        public class QueryProjectionDto
        {
            public int ContactId { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public string CompanyName { get; set; }
            public DateTime CreatedDateTime { get; set; }
            public DateTime CreatedDate { get; set; }
            public int CreatedByUserId { get; set; }
            public int AddressId { get; set; }
            public int? DocumentId { get; set; }
            public int NoteId { get; set; }
            public int ConcurrencyVersion { get; set; }
        }
    }
}
