using System.Threading.Tasks;
using Xunit;
using vNext.API.Features.Concurrencies;
using vNext.Core.Extensions;

namespace IntegrationTests.Features
{
    public class ConcurrencyScenarios: ConcurrencyScenarioBase
    {
        [Fact]
        public async Task ShouldSaveConcurrency()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .PostAsAsync<ConcurrencySaveCommand.Request, ConcurrencySaveCommand.Response>(Post.Concurrencies,new ConcurrencySaveCommand.Request() {
                        Concurrency = new ConcurrencyDto()
                        {
                            Domain= "Region",
                            Id = 1,
                            Version = 1
                        }
                    });

                Assert.True(response.ConcurrencyId != default(int));
            }
        }

        [Fact]
        public async Task ShouldTruncate()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .DeleteAsync(Delete.Truncate);

                response.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task GetVersionByDomainAndId() {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<ConcurrencyGetVersionByDomainAndIdQuery.Response>(Get.GetVersionByDomainAndIdQuery(0, "Region", 1));
                
                Assert.True(response.Version != default(int));
            }
        }
    }
}
