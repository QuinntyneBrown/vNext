using System.Threading.Tasks;
using Xunit;
using vNext.API.Features.SalesOrders;
using vNext.Core.Extensions;

namespace IntegrationTests.Features
{
    public class SalesOrderScenarios: SalesOrderScenarioBase
    {
        [Fact]
        public async Task ShouldGetSalesOrderById()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetSalesOrderByIdQuery.Response>(Get.GetById(1));

                Assert.True(response.SalesOrder.SalesOrderId == 1);
            }
        }

        [Fact]
        public async Task ShouldSaveSalesOrder()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .PostAsAsync<SaveSalesOrderCommand.Request, SaveSalesOrderCommand.Response>(Post.SalesOrders,new SaveSalesOrderCommand.Request() {
                        SalesOrder = new SalesOrderDto()
                        {

                        }
                    });

                Assert.True(response.SalesOrderId == 3013);
            }
        }

        [Fact]
        public async Task ShouldDeleteSalesOrder()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetSalesOrderByIdQuery.Response>(Get.GetById(3011));

                Assert.True(response.SalesOrder.SalesOrderId == 1);
            }
        }
    }
}
