using Dapper;
using FluentValidation;
using MediatR;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using vNext.Core.Common;
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

        public class Request : Core.Common.AuthenticatedRequest, IRequest<Response> {
            public RegionDto Region { get; set; }
        }

        public class Response
        {
            public Response(short regionId) => RegionId = regionId;

            public short RegionId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDbConnectionManager _dbConnectionManager;
            private readonly IMediator _mediator;
            public Handler(
                IDbConnectionManager dbConnectionManager,
                IMediator mediator
                )
            {
                _mediator = mediator;                
                _dbConnectionManager = dbConnectionManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var response = default(Response);
                
                using (var connection = _dbConnectionManager.GetConnection(request.CustomerKey))
                {
                    connection.Open();

                    response = new Response(await SaveEntityCommandHandler.Handle(
                        request,
                        Procedure.ExecuteAsync,
                        "Region",
                        request.Region.RegionId,
                        request.Region.ConcurrencyVersion,
                        connection));
                }
                
                return response;
            }
        }

        public static class Procedure
        {
            public static async Task<short> ExecuteAsync(Request request, IDbConnection connection)
            {
                var noteId = await new Notes.SaveNoteCommand.Prodcedure().ExecuteAsync(new Notes.SaveNoteCommand.Request() {
                    Note = request.Region.Note
                }, connection);

                var dynamicParameters = new DynamicParameters();

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

                if (request.Region.RegionId == default(int))
                    dynamicParameters.AddDynamicParams(new
                    {
                        CreatedByUserId = request.UserId,
                        CreatedDateTime = request.CurrentDateTime
                    });

                var parameterDirection = request.Region.RegionId == 0 ? ParameterDirection.Output : ParameterDirection.InputOutput;

                dynamicParameters.Add("RegionId", request.Region.RegionId, DbType.Int16, parameterDirection);

                await connection.ExecuteProcAsync("[Common].[ProcRegionSave]", dynamicParameters);

                return dynamicParameters.Get<short>("@RegionId");

            }
        }
    }
}
