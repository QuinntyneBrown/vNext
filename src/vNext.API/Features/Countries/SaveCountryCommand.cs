using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Data;
using Dapper;
using vNext.Core.Interfaces;
using vNext.Core.Extensions;
using FluentValidation;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Countries
{
    public class SaveCountryCommand
    {

        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.Country.CountryId).NotNull();
            }
        }

        public class Request : IRequest<Response> {
            public CountryDto Country { get; set; }
        }

        public class Response
        {			
            public int CountryId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISqlConnectionManager _sqlConnectionManager;
            public Handler( ISqlConnectionManager sqlConnectionManager)
            {
                _sqlConnectionManager = sqlConnectionManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _sqlConnectionManager.GetConnection())
                {
                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.AddDynamicParams(new
                    {
                        request.Country.CountryId,
                        request.Country.Code2
                    });

                    var parameterDirection = request.Country.CountryId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                    dynamicParameters.Add("CountryId",  request.Country.CountryId, DbType.Int16, parameterDirection);
                        
                    await connection.ExecuteProcAsync("[Comsense].[ProcCountrySave]", dynamicParameters);
                    
                    return new Response()
                    {
                        CountryId = dynamicParameters.Get<short>("@CountryId")
                    };
                }
            }
        }
    }
}
