using BenchmarkDotNet.Attributes;
using System.Threading;
using vNext.API.Features.Notes;
using vNext.API.Features.Regions;
using vNext.Infrastructure.Data;

namespace Benchmarks
{
    public class RegionBenchmarks
    {
        private readonly SaveRegionCommand.Handler _saveHandler;
        private readonly GetRegionByIdQuery.Handler _getByIdhandler;
        public RegionBenchmarks()
        {
            var configuration = ConfigurationProvider.Get();
            _saveHandler = new SaveRegionCommand.Handler(new SqlConnectionManager(configuration),new SaveRegionCommand.Procedure(new SaveNoteCommand.Prodcedure()));
            _getByIdhandler = new GetRegionByIdQuery.Handler(new SqlConnectionManager(configuration));
        }

        [Benchmark]
        public void SaveCommand() {

            var response = _getByIdhandler.Handle(new GetRegionByIdQuery.Request() { RegionId = 1 }, default(CancellationToken))
                .GetAwaiter().GetResult();

            _saveHandler.Handle(new SaveRegionCommand.Request()
            {
                Region = response.Region
            }, default(CancellationToken)).GetAwaiter().GetResult();
        }
    }
}
