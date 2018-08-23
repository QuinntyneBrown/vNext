using Dapper;
using FluentValidation;
using MediatR;
using System.Data;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Common;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.SalesOrders
{
    public class SaveSalesOrderCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.SalesOrder.SalesOrderId).NotNull();
            }
        }

        public class Request : AuthenticatedRequest, IRequest<Response> {
            public SalesOrderDto SalesOrder { get; set; }
        }

        public class Response
        {			
            public int SalesOrderId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            private readonly IProcedure<Request, short> _procedure;
            public Handler(IDbConnectionManager dbConnectionManager, IProcedure<Request, short> procedure)
            {
                _dbConnectionManager = dbConnectionManager;
                _procedure = procedure;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    return new Response()
                    {
                        SalesOrderId = await _procedure.ExecuteAsync(request,connection)
                    };
                }
            }
        }

        public class Procedure
        {
            public async Task<short> ExecuteAsync(Request request, IDbConnection connection)
            {
                var dynamicParameters = new DynamicParameters();

                var parameterDirection = request.SalesOrder.SalesOrderId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                dynamicParameters.AddDynamicParams(new
                {
                    request.SalesOrder.SalesOrderId,
                    request.SalesOrder.Code
                });

                dynamicParameters.Add("SalesOrderId", request.SalesOrder.SalesOrderId, DbType.Int16, parameterDirection);

                await connection.ExecuteProcAsync("[SalesOrder].[ProcSalesOrderSave]", dynamicParameters);

                return dynamicParameters.Get<short>("@SalesOrderId");
            }
        }    
        
    }
}
