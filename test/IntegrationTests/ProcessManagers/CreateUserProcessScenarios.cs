using MediatR;
using Microsoft.Extensions.DependencyInjection;
using vNext.API.ProcessManagers;
using vNext.Core.DomainEvents;
using Xunit;

namespace IntegrationTests.ProcessManagers
{
    public class CreateUserProcessScenarios: ScenarioBase
    {
        public CreateUserProcessScenarios()
        {

        }

        [Fact]
        public void ShouldCreateUser()
        {
            using (var server = CreateServer())
            {
                var services = (IServiceScopeFactory)server.Host.Services.GetService(typeof(IServiceScopeFactory));

                using(var scope = services.CreateScope())
                {
                    CreateUserProcess createUserProcess = (CreateUserProcess)scope.ServiceProvider.GetRequiredService<INotificationHandler<UserCreationRequested>>();
                    
                    Assert.NotNull(createUserProcess);
                }
            }
        }
    }
}
