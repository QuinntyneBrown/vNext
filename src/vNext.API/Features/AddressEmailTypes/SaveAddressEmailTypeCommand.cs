using Dapper;
using FluentValidation;
using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.AddressEmailTypes
{
    public class SaveAddressEmailTypeCommand
    {

        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.AddressEmailType.AddressEmailTypeId).NotNull();
            }
        }

        public class Request : AuthenticatedRequest, IRequest<Response> {
            public AddressEmailTypeDto AddressEmailType { get; set; }
        }

        public class Response
        {			
            public int AddressEmailTypeId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            public Handler(IDbConnectionManager dbConnectionManager)
                => _dbConnectionManager = dbConnectionManager;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.AddDynamicParams(new
                    {
                        request.AddressEmailType.AddressEmailTypeId,
                        request.AddressEmailType.Description
                    });

                    var parameterDirection = request.AddressEmailType.AddressEmailTypeId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                    dynamicParameters.Add("AddressEmailTypeId",  request.AddressEmailType.AddressEmailTypeId, DbType.Int16, parameterDirection);
                        
                    await connection.ExecuteProcAsync("[Comsense].[ProcAddressEmailTypeSave]", dynamicParameters);
                    
                    return new Response()
                    {
                        AddressEmailTypeId = dynamicParameters.Get<short>("@AddressEmailTypeId")
                    };
                }
            }
        }
    }
}
