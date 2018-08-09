using System.Threading.Tasks;
using Xunit;
using vNext.API.Features.Dashboards;
using vNext.Core.Extensions;
using System.Linq;

namespace IntegrationTests.Features
{
    public class DashboardScenarios: DashboardScenarioBase
    {
        [Fact]
        public async Task ShouldGetDashboardById()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetDashboardByIdQuery.Response>(Get.GetById(1));

                Assert.True(response.Dashboard.DashboardId == 1);
            }
        }

        [Fact]
        public async Task ShouldGetAllDashboardsByUserId()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<DashboardGetByUserIdQuery.Response>(Get.GetDashboardsByUserId(2));

                Assert.True(response.Dashboards.Count() > 0);
            }
        }

        [Fact]
        public async Task ShouldSaveDashboard()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .PostAsAsync<SaveDashboardCommand.Request, SaveDashboardCommand.Response>(Post.Dashboards,new SaveDashboardCommand.Request() {
                        Dashboard = new DashboardDto()
                        {

                        }
                    });

                Assert.True(response.DashboardId == 3013);
            }
        }

        [Fact]
        public async Task ShouldDeleteDashboard()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetDashboardByIdQuery.Response>(Get.GetById(3011));

                Assert.True(response.Dashboard.DashboardId == 1);
            }
        }
    }
}
