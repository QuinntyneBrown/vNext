using System.Threading.Tasks;
using Xunit;
using vNext.API.Features.AuditLogs;
using vNext.Core.Extensions;
using System.Linq;
using System;

namespace IntegrationTests.Features
{
    public class AuditLogScenarios: AuditLogScenarioBase
    {
        [Fact]
        public async Task ShouldSaveAuditLog()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .PostAsAsync<SaveAuditLogCommand.Request, SaveAuditLogCommand.Response>(Post.AuditLogs,new SaveAuditLogCommand.Request() {
                        AuditLog = new AuditLogDto()
                        {

                        }
                    });

                Assert.True(response.AuditLogId == 3013);
            }
        }

        [Fact]
        public async Task ShouldGetAuditLogs()
        {
            string from = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd");
            string to = DateTime.Now.ToString("yyyy-MM-dd");

            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetAuditLogsQuery.Response>(Get.AuditLogs("Dashboard",8,2,from,to));

                Assert.True(response.AuditLogs.Count() > 0);
            }
        }
    }
}
