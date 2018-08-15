using Dapper;
using FluentValidation;
using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.CountrySubdivisions
{
    public class SaveCountrySubdivisionCommand
    {

        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.CountrySubdivision.CountrySubdivisionId).NotNull();
            }
        }

        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response> {
            public CountrySubdivisionDto CountrySubdivision { get; set; }
        }

        public class Response
        {			
            public int CountrySubdivisionId { get; set; }
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
                        request.CountrySubdivision.CountrySubdivisionId,
                        request.CountrySubdivision.Code
                    });

                    var parameterDirection = request.CountrySubdivision.CountrySubdivisionId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                    dynamicParameters.Add("CountrySubdivisionId",  request.CountrySubdivision.CountrySubdivisionId, DbType.Int16, parameterDirection);
                        
                    await connection.ExecuteProcAsync("[Comsense].[ProcCountrySubdivisionSave]", dynamicParameters);
                    
                    return new Response()
                    {
                        CountrySubdivisionId = dynamicParameters.Get<short>("@CountrySubdivisionId")
                    };
                }
            }
        }
    }
}
