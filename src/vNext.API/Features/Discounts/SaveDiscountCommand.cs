using Dapper;
using FluentValidation;
using MediatR;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Discounts
{
    public class SaveDiscountCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.Discount.DiscountId).NotNull();
            }
        }

        public class Request : IRequest<Response> {
            public DiscountDto Discount { get; set; }
        }

        public class Response
        {			
            public int DiscountId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISqlConnectionManager _sqlConnectionManager;
            public Handler( ISqlConnectionManager sqlConnectionManager)
                => _sqlConnectionManager = sqlConnectionManager;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _sqlConnectionManager.GetConnection())
                {
                    return new Response()
                    {
                        DiscountId = await Procedure.ExecuteAsync(request,connection)
                    };
                }
            }
        }

        public static class Procedure
        {
            public static async Task<short> ExecuteAsync(Request request, SqlConnection connection)
            {
                var dynamicParameters = new DynamicParameters();

                var parameterDirection = request.Discount.DiscountId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                dynamicParameters.AddDynamicParams(new
                {
                    request.Discount.DiscountId,
                    request.Discount.Code
                });

                dynamicParameters.Add("DiscountId", request.Discount.DiscountId, DbType.Int16, parameterDirection);

                await connection.ExecuteProcAsync("[Common].[ProcDiscountSave]", dynamicParameters);

                return dynamicParameters.Get<short>("@DiscountId");
            }
        }    
        
    }
}
