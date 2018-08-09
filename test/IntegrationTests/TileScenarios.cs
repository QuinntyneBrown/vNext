using System.Threading.Tasks;
using Xunit;
using vNext.API.Features.Tiles;
using vNext.Core.Extensions;

namespace IntegrationTests.Features
{
    public class TileScenarios: TileScenarioBase
    {
        [Fact]
        public async Task ShouldGetTileById()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetTileByIdQuery.Response>(Get.GetById(1));

                Assert.True(response.Tile.TileId == 1);
            }
        }

        [Fact]
        public async Task ShouldSaveTile()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .PostAsAsync<SaveTileCommand.Request, SaveTileCommand.Response>(Post.Tiles,new SaveTileCommand.Request() {
                        Tile = new TileDto()
                        {

                        }
                    });

                Assert.True(response.TileId == 3013);
            }
        }

        [Fact]
        public async Task ShouldDeleteTile()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetTileByIdQuery.Response>(Get.GetById(3011));

                Assert.True(response.Tile.TileId == 1);
            }
        }
    }
}
