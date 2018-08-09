using Dapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using vNext.Core;
using vNext.Core.DomainEvents;
using vNext.Core.Extensions;
using vNext.Core.Interfaces;

namespace vNext.API.Features.Regions
{
    public class SaveRegionCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.Region.RegionId).NotNull();
                RuleFor(request => request.Region.Note).NotNull();
            }
        }

        public class Request : IRequest<Response>, ILoggableRequest {
            public RegionDto Region { get; set; }
            public int CreatedByUserId { get; set; }
            public DateTime CreatedDateTime { get; set; }
        }

        public class Response
        {
            public Response(short regionId) => RegionId = regionId;

            public short RegionId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISqlConnectionManager _sqlConnectionManager;
            private readonly IMediator _mediator;
            public Handler(
                ISqlConnectionManager sqlConnectionManager,
                IMediator mediator
                )
            {
                _mediator = mediator;                
                _sqlConnectionManager = sqlConnectionManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var result = default(short);


                using (var connection = _sqlConnectionManager.GetConnection())
                {
                    connection.Open();

                    result = await SaveEntityCommandHandler.Handle(request, (x, y) => Procedure.ExecuteAsync(x, y), "Region", request.Region.RegionId, request.Region.ConcurrencyVersion, connection);
                }



                await _mediator?.Publish(new EntitySaved("Region", result));

                return new Response(result);
            }
        }

        public static class Procedure
        {
            public static async Task<short> ExecuteAsync(Request request, SqlConnection connection)
            {

                var noteId = await new Notes.SaveNoteCommand.Prodcedure().ExecuteAsync(0, request.Region.Note.Note, connection);

                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var dynamicParameters = new DynamicParameters();

                    if (request.Region.RegionId == default(int))
                    {
                        request.Region.CreatedDateTime = request.CreatedDateTime;
                        request.Region.CreatedByUserId = request.CreatedByUserId;
                    }
                    
                    dynamicParameters.AddDynamicParams(new
                    {
                        request.Region.RegionId,
                        request.Region.Code,
                        request.Region.Description,
                        request.Region.CreatedDateTime,
                        request.Region.CreatedByUserId,
                        request.Region.Sort,
                        noteId
                    });

                    var parameterDirection = request.Region.RegionId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                    dynamicParameters.Add("RegionId", request.Region.RegionId, DbType.Int16, parameterDirection);

                    await connection.ExecuteProcAsync("[Common].[ProcRegionSave]", dynamicParameters);

                    transaction.Complete();

                    return dynamicParameters.Get<short>("@RegionId");

                }

            }
        }
    }
}
