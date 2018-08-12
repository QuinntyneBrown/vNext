using MediatR;
using System;
using System.Data;
using System.Threading.Tasks;
using vNext.API.Features.Concurrencies;
using vNext.Core.DomainEvents;
using vNext.Core.Interfaces;

namespace vNext.API.Features
{
    public class SubmitCommandHandler: ISubmitCommandHandler
    {
        private readonly IMediator _mediator;

        public SubmitCommandHandler(IMediator mediator)
            => _mediator = mediator;

        public async Task<TResponse> Handle<TRquest, TResponse>(TRquest request, Func<TRquest, IDbConnection, Task<TResponse>> procedure, string domain, int id, int version, IDbConnection connection)
        {
            var newVersion = (await ConcurrencyGetVersionByDomainAndIdQuery.Procedure
                .ExecuteAsync(new ConcurrencyGetVersionByDomainAndIdQuery.Request()
                {
                    Id = id,
                    Version = version,
                    Domain = domain
                }, connection));

            if (version < newVersion)
                throw new Exception("Concurrency Error");

            var result = await procedure(request, connection);

            await ConcurrencySaveCommand.Procedure
                .ExecuteAsync(new ConcurrencySaveCommand.Request()
                {
                    Concurrency = new ConcurrencyDto()
                    {
                        Domain = domain,
                        Id = Convert.ToInt32(result),
                        Version = version,
                    }
                }, connection);

            await _mediator?.Publish(new EntitySaved(request.GetType().Name, result));

            return result;
        }
    }
}
