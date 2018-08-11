using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Data;
using Dapper;
using vNext.Core.Interfaces;
using vNext.Core.Extensions;
using FluentValidation;
using vNext.Core.Interfaces;

namespace vNext.API.Features.CountrySubDivisions
{
    public class SaveCountrySubDivisionCommand
    {

        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.CountrySubDivision.CountrySubDivisionId).NotNull();
            }
        }

        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response> {
            public CountrySubDivisionDto CountrySubDivision { get; set; }
        }

        public class Response
        {			
            public int CountrySubDivisionId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            public Handler( IDbConnectionManager dbConnectionManager)
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
                        request.CountrySubDivision.CountrySubDivisionId,
                        request.CountrySubDivision.Code
                    });

                    var parameterDirection = request.CountrySubDivision.CountrySubDivisionId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                    dynamicParameters.Add("CountrySubDivisionId",  request.CountrySubDivision.CountrySubDivisionId, DbType.Int16, parameterDirection);
                        
                    await connection.ExecuteProcAsync("[Comsense].[ProcCountrySubDivisionSave]", dynamicParameters);
                    
                    return new Response()
                    {
                        CountrySubDivisionId = dynamicParameters.Get<short>("@CountrySubDivisionId")
                    };
                }
            }
        }
    }
}
