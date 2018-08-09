using System.Threading.Tasks;
using Xunit;
using vNext.API.Features.Statuses;
using vNext.Core.Extensions;
using System.Linq;

namespace IntegrationTests.Features
{
    public class StatusScenarios: StatusScenarioBase
    {
        [Fact]
        public async Task ShouldGetAll()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetStatusesQuery.Response>(Get.Statuses);

                Assert.True(response.Statuses.Count() > 0);
            }
        }

        [Fact]
        public async Task ShouldGetStatusById()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetStatusByIdQuery.Response>(Get.GetById(1));

                Assert.True(response.Status.StatusId == 1);
            }
        }

        [Fact]
        public async Task ShouldSaveStatus()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .PostAsAsync<SaveStatusCommand.Request, SaveStatusCommand.Response>(Post.Statuses,new SaveStatusCommand.Request() {
                        Status = new StatusDto()
                        {

                        }
                    });

                Assert.True(response.StatusId == 3013);
            }
        }

        [Fact]
        public async Task ShouldDeleteStatus()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetStatusByIdQuery.Response>(Get.GetById(3011));

                Assert.True(response.Status.StatusId == 1);
            }
        }
    }
}
