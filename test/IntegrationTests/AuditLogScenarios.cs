using System.Threading.Tasks;
using Xunit;
using vNext.API.Features.AuditLogs;
using vNext.Core.Extensions;

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
    }
}
