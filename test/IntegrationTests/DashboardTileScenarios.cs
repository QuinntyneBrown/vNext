using System.Threading.Tasks;
using Xunit;
using vNext.API.Features.DashboardTiles;
using vNext.Core.Extensions;
using vNext.API.Features.Dashboards;
using System.Linq;

namespace IntegrationTests.Features
{
    public class DashboardTileScenarios: DashboardTileScenarioBase
    {
        [Fact]
        public async Task ShouldGetDashboardTileById()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetDashboardTileByIdQuery.Response>(Get.GetById(1));

                Assert.True(response.DashboardTile.DashboardTileId == 1);
            }
        }

        [Fact]
        public async Task ShouldSaveDashboardTile()
        {
            using (var server = CreateServer())
            {
                var client = server.CreateClient();
                var response = await client
                    .PostAsAsync<SaveDashboardTileCommand.Request, SaveDashboardTileCommand.Response>(Post.DashboardTiles,new SaveDashboardTileCommand.Request() {
                        DashboardTile = new DashboardTileDto()
                        {

                        }
                    });

                Assert.True(response.DashboardTileId == 3013);
            }
        }

        [Fact]
        public async Task ShouldDeleteDashboardTile()
        {
            using (var server = CreateServer())
            {
                var client = server.CreateClient();

                var dashboard = (await client.GetAsync<DashboardGetByUserIdQuery.Response>(DashboardScenarioBase.Get.GetDashboardsByUserId(2))).Dashboards.First();

                var dashboardTile = dashboard.DashboardTiles.First();

                var response = await client
                    .DeleteAsync<RemoveDashboardTileCommand.Response>(Delete.DashboardTile(dashboardTile.DashboardTileId,dashboardTile.ConcurrencyVersion));


                Assert.True(response.DashboardTileId == -1);
            }
        }
    }
}
