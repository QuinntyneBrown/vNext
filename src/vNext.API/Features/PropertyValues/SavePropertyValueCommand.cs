using Dapper;
using FluentValidation;
using MediatR;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.PropertyValues
{
    public class SavePropertyValueCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.PropertyValue.PropertyValueId).NotNull();
            }
        }

        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response> {
            public PropertyValueDto PropertyValue { get; set; }
        }

        public class Response
        {			
            public int PropertyValueId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            public Handler( IDbConnectionManager dbConnectionManager)
                => _dbConnectionManager = dbConnectionManager;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    return new Response()
                    {
                        PropertyValueId = await Procedure.ExecuteAsync(request,connection)
                    };
                }
            }
        }

        public static class Procedure
        {
            public static async Task<short> ExecuteAsync(Request request, System.Data.IDbConnection connection)
            {
                var dynamicParameters = new DynamicParameters();

                var parameterDirection = request.PropertyValue.PropertyValueId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                dynamicParameters.AddDynamicParams(new
                {
                    request.PropertyValue.PropertyValueId,
                    request.PropertyValue.Code
                });

                dynamicParameters.Add("PropertyValueId", request.PropertyValue.PropertyValueId, DbType.Int16, parameterDirection);

                await connection.ExecuteProcAsync("[Common].[ProcPropertyValueSave]", dynamicParameters);

                return dynamicParameters.Get<short>("@PropertyValueId");
            }
        }    
        
    }
}
