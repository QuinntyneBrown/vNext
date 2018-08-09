using Dapper;
using FluentValidation;
using MediatR;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Contacts
{
    public class SaveContactCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.Contact.ContactId).NotNull();
            }
        }

        public class Request : IRequest<Response> {
            public ContactDto Contact { get; set; }
        }

        public class Response
        {			
            public int ContactId { get; set; }
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
                        ContactId = await Procedure.ExecuteAsync(request,connection)
                    };
                }
            }
        }

        public static class Procedure
        {
            public static async Task<int> ExecuteAsync(Request request, SqlConnection connection)
            {
                var dynamicParameters = new DynamicParameters();

                request.Contact.CreatedDateTime = request.Contact.ContactId == 0 ? DateTime.Now : request.Contact.CreatedDateTime;

                dynamicParameters.AddDynamicParams(new
                {
                    request.Contact.ContactId,
                    request.Contact.FirstName,
                    request.Contact.MiddleName,
                    request.Contact.LastName,
                    request.Contact.CompanyName,
                    request.Contact.CreatedDateTime,
                    request.Contact.CreatedByUserId,
                    request.Contact.AddressId,
                    request.Contact.DocumentId,
                    request.Contact.NoteId
                });

                var parameterDirection = request.Contact.ContactId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                dynamicParameters.Add("ContactId", request.Contact.ContactId, DbType.Int16, parameterDirection);

                await connection.ExecuteProcAsync("[Common].[ProcContactSave]", dynamicParameters);

                return dynamicParameters.Get<short>("@ContactId");
            }
        }    
        
    }
}
