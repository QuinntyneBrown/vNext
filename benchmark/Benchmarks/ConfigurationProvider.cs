using Microsoft.Extensions.Configuration;
using System.Reflection;
using vNext.API;

namespace Benchmarks
{
    public static class ConfigurationProvider
    {
        public static IConfiguration Get()
            => new ConfigurationBuilder()
                        .AddUserSecrets(typeof(Startup).GetTypeInfo().Assembly)
                        .Build();
    }
}
