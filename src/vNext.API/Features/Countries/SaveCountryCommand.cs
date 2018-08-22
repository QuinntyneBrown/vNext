using Dapper;
using FluentValidation;
using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
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

        public class Request : AuthenticatedRequest, IRequest<Response> {
            public CountryDto Country { get; set; }
        }

        public class Response
        {			
            public int CountryId { get; set; }
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

        public class Procedure : IProcedure<Request, short>
        {
            public async Task<short> ExecuteAsync(Request request, IDbConnection connection)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
