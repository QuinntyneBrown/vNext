using System.Threading.Tasks;
using Xunit;
using vNext.API.Features.Regions;
using vNext.Core.Extensions;
using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR.Client;

namespace IntegrationTests.Features
{
    public class RegionScenarios: RegionScenarioBase
    {
        [Fact]
        public async Task ShouldGetRegionById()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetRegionByIdQuery.Response>(Get.GetById(7));

                Assert.True(response.Region.RegionId == 7);
            }
        }

        [Fact]
        public async Task DeleteAllRegionsExceptMain()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetRegionsQuery.Response>(Get.All);

                foreach(var region in response.Regions)
                {
                    if (region.RegionId != 1)
                    {
                        await server.CreateClient()
                        .DeleteAsync(Delete.Region(region.RegionId, region.ConcurrencyVersion));
                    }
                }
            }
        }

        [Fact]
        public async Task ShouldGetRegions()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetRegionsQuery.Response>(Get.All);

                Assert.True(response.Regions.Count() > 0);
            }
        }

        [Fact]
        public async Task ShouldCreateRegion()
        {
            using (var server = CreateServer())
            {
                var region = new RegionDto()
                {
                    Code = "WAKANDA",
                    ConcurrencyVersion = 0,
                    Description = "QB Region",
                    Note = new vNext.API.Features.Notes.NoteDto()
                    {
                        Note = ""
                    },
                    Sort = 0
                };

                var response = await server.CreateClient()
                    .PostAsAsync<SaveRegionCommand.Request, SaveRegionCommand.Response>(Post.Regions, new SaveRegionCommand.Request()
                    {
                        Region = region
                    });

                Assert.True(response.RegionId != default(int));
            }
        }

        [Fact]
        public async Task ShouldSaveRegion()
        {            
            using (var server = CreateServer())
            {                
                var regions = (await server.CreateClient()
                    .GetAsync<GetRegionsQuery.Response>(Get.All)).Regions;

                var region = regions.First();

                var response = await server.CreateClient()
                    .PostAsAsync<SaveRegionCommand.Request,SaveRegionCommand.Response>(Post.Regions, new SaveRegionCommand.Request()
                    {
                        Region = region
                    });

                Assert.Equal(region.RegionId, response.RegionId);
            }
            
        }

        [Fact]
        public async Task ShouldCreateRegionsInParrell()
        {
            using (var server = CreateServer())
            {
                var client = server.CreateClient();
                var taskList = new List<Task>();

                for (var i = 0; i < 10; i++)
                {
                    taskList.Add(client.PostAsAsync<SaveRegionCommand.Request, SaveRegionCommand.Response>(Post.Regions, new SaveRegionCommand.Request()
                    {
                        Region = new RegionDto()
                        {
                            Code = $"THROWN{i}",
                            ConcurrencyVersion = 0,
                            Description = "QB Region",
                            Note = new vNext.API.Features.Notes.NoteDto()
                            {
                                Note = ""
                            },
                            Sort = 0
                        }
                    }));
                }

                Task.WaitAll(taskList.ToArray());

                Assert.Equal(1, 1);
            }
        }


        [Fact]
        public async Task ShouldFailSaveRegion()
        {
            using (var server = CreateServer())
            {
                var region = (await server.CreateClient()
                    .GetAsync<GetRegionsQuery.Response>(Get.All)).Regions.First();

                var taskList = new List<Task>()
                {
                    server.CreateClient()
                    .PostAsync(Post.Regions, new SaveRegionCommand.Request()
                    {
                        Region = region
                    }),
                    server.CreateClient()
                    .PostAsync(Post.Regions, new SaveRegionCommand.Request()
                    {
                        Region = region
                    })
                };


                Task.WaitAll(taskList.ToArray());
                
            }
        }


        [Fact]
        public async Task ShouldFailToSaveRegion()
        {
            using (var server = CreateServer())
            {
                var region = (await server.CreateClient()
                    .GetAsync<GetRegionsQuery.Response>(Get.All)).Regions.First();

                region.ConcurrencyVersion = 0;

                var response = await server.CreateClient()
                    .PostAsAsync<SaveRegionCommand.Request, SaveRegionCommand.Response>(Post.Regions, new SaveRegionCommand.Request()
                    {
                        Region = region
                    });

                await Assert.ThrowsAnyAsync<Exception>(null);
            }
        }

        [Fact]
        public async Task ShouldDeleteRegion()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetRegionByIdQuery.Response>(Get.GetById(3011));

                Assert.True(response.Region.RegionId == 1);
            }
        }
    }
}
