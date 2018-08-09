using BenchmarkDotNet.Attributes;
using System.Threading;
using vNext.API.Features.Users;
using vNext.Core.Identity;
using vNext.Infrastructure.Data;

namespace Benchmarks
{
    public class UserBenchmarks
    {
        private readonly AuthenticateCommand.Handler _handler;

        public UserBenchmarks()
        {
            var configuration = ConfigurationProvider.Get();
            _handler = new AuthenticateCommand.Handler(
                new SqlConnectionManager(configuration),
                new SecurityTokenFactory(configuration));
        }

        [Benchmark]
        public void AuthenticateCommand()
            => _handler.Handle(new AuthenticateCommand.Request()
            {
                Code = "Comsense1",
                Password = "Comsense1;;"
            }, default(CancellationToken)).GetAwaiter().GetResult();
    }
}
