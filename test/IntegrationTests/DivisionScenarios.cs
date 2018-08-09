using System.Threading.Tasks;
using Xunit;
using vNext.API.Features.Divisions;
using vNext.Core.Extensions;

namespace IntegrationTests.Features
{
    public class DivisionScenarios: DivisionScenarioBase
    {
        [Fact]
        public async Task ShouldGetDivisionById()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetDivisionByIdQuery.Response>(Get.GetById(1));

                Assert.True(response.Division.DivisionId == 1);
            }
        }

        [Fact]
        public async Task ShouldSaveDivision()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .PostAsAsync<SaveDivisionCommand.Request, SaveDivisionCommand.Response>(Post.Divisions,new SaveDivisionCommand.Request() {
                        Division = new DivisionDto()
                        {

                        }
                    });

                Assert.True(response.DivisionId == 3013);
            }
        }

        [Fact]
        public async Task ShouldDeleteDivision()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetDivisionByIdQuery.Response>(Get.GetById(3011));

                Assert.True(response.Division.DivisionId == 1);
            }
        }
    }
}
