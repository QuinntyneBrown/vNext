using System.Threading.Tasks;
using Xunit;
using vNext.API.Features.Shipments;
using vNext.Core.Extensions;

namespace IntegrationTests.Features
{
    public class ShipmentScenarios: ShipmentScenarioBase
    {
        [Fact]
        public async Task ShouldGetShipmentById()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetShipmentByIdQuery.Response>(Get.GetById(1));

                Assert.True(response.Shipment.ShipmentId == 1);
            }
        }

        [Fact]
        public async Task ShouldSaveShipment()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .PostAsAsync<SaveShipmentCommand.Request, SaveShipmentCommand.Response>(Post.Shipments,new SaveShipmentCommand.Request() {
                        Shipment = new ShipmentDto()
                        {

                        }
                    });

                Assert.True(response.ShipmentId == 3013);
            }
        }

        [Fact]
        public async Task ShouldDeleteShipment()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetShipmentByIdQuery.Response>(Get.GetById(3011));

                Assert.True(response.Shipment.ShipmentId == 1);
            }
        }
    }
}
