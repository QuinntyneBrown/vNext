using Dapper;
using FluentValidation;
using MediatR;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
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

        public class Request : AuthenticatedRequest, IRequest<Response> {
            public ContactDto Contact { get; set; }
        }

        public class Response
        {			
            public int ContactId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            private readonly IProcedure<Request, short> _procedure;
            public Handler( IDbConnectionManager dbConnectionManager)
            {
                _dbConnectionManager = dbConnectionManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    return new Response()
                    {
                        ContactId = await Procedure.ExecuteAsync(request,connection)
                    };
                }
            }
        }

        public class Procedure
        {
            public static async Task<int> ExecuteAsync(Request request, IDbConnection connection)
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
