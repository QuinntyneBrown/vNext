using Dapper;
using FluentValidation;
using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Companies
{
    public class SaveCompanyCommand
    {

        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.Company.CompanyId).NotNull();
            }
        }

        public class Request : AuthenticatedRequest, IRequest<Response> {
            public CompanyDto Company { get; set; }
        }

        public class Response
        {			
            public int CompanyId { get; set; }
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
                        request.Company.CompanyId,
                        request.Company.Code
                    });

                    var parameterDirection = request.Company.CompanyId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                    dynamicParameters.Add("CompanyId",  request.Company.CompanyId, DbType.Int16, parameterDirection);
                        
                    await connection.ExecuteProcAsync("[Common].[ProcCompanySave]", dynamicParameters);
                    
                    return new Response()
                    {
                        CompanyId = dynamicParameters.Get<short>("@CompanyId")
                    };
                }
            }
        }
    }
}
