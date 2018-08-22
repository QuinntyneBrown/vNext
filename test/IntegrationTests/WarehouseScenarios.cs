using System.Threading.Tasks;
using Xunit;
using vNext.API.Features.Warehouses;
using vNext.Core.Extensions;
using System.Linq;
using System;

namespace IntegrationTests.Features
{
    public class WarehouseScenarios: WarehouseScenarioBase
    {
        [Fact]
        public async Task ShouldGetWarehouseById()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetWarehouseByIdQuery.Response>(Get.GetById(1));

                Assert.True(response.Warehouse.WarehouseId == 1);
            }
        }

        [Fact]
        public async Task ShouldGetWarehouses()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetWarehousesQuery.Response>(Get.Warehouses);

                Assert.True(response.Warehouses.Count() > 0);
            }
        }

        [Fact]
        public async Task ShouldSaveWarehouse()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .PostAsAsync<SaveWarehouseCommand.Request, SaveWarehouseCommand.Response>(Post.Warehouses,new SaveWarehouseCommand.Request() {
                        Warehouse = new WarehouseDto()
                        {

                        }
                    });

                Assert.True(response.WarehouseId == 3013);
            }
        }

        [Fact]
        public async Task ShouldDeleteWarehouse()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetWarehouseByIdQuery.Response>(Get.GetById(3011));

                Assert.True(response.Warehouse.WarehouseId == 1);
            }
        }
    }
}
