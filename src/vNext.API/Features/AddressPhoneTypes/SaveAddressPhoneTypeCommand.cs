using Dapper;
using FluentValidation;
using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.AddressPhoneTypes
{
    public class SaveAddressPhoneTypeCommand
    {

        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.AddressPhoneType.AddressPhoneTypeId).NotNull();
            }
        }

        public class Request : AuthenticatedRequest, IRequest<Response> {
            public AddressPhoneTypeDto AddressPhoneType { get; set; }
        }

        public class Response
        {			
            public int AddressPhoneTypeId { get; set; }
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
                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.AddDynamicParams(new
                    {
                        request.AddressPhoneType.AddressPhoneTypeId,
                        request.AddressPhoneType.Description
                    });

                    var parameterDirection = request.AddressPhoneType.AddressPhoneTypeId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                    dynamicParameters.Add("AddressPhoneTypeId",  request.AddressPhoneType.AddressPhoneTypeId, DbType.Int16, parameterDirection);
                        
                    await connection.ExecuteProcAsync("[Comsense].[ProcAddressPhoneTypeSave]", dynamicParameters);
                    
                    return new Response()
                    {
                        AddressPhoneTypeId = dynamicParameters.Get<short>("@AddressPhoneTypeId")
                    };
                }
            }
        }

        public class Procedure : IProcedure<Request, short>
        {
            public async Task<short> ExecuteAsync(Request request, IDbConnection connection)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
