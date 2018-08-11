using System.Linq;
using System.Threading.Tasks;
using vNext.API.Features.Users;
using vNext.Core.Extensions;
using Xunit;

namespace IntegrationTests
{
    public class UserScenarios: UserScenarioBase
    {

        [Fact]
        public async Task ShouldAuthenticateUser()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .PostAsAsync<AuthenticateCommand.Request, AuthenticateCommand.Response>(Post.Token, new AuthenticateCommand.Request()
                    {
                        Code = "Comsense1",
                        Password = "Comsense1;;",
                        CustomerKey = "QUINNTYNE_DEV"
                    });

                Assert.NotEqual(default(int), response.UserId);
                Assert.True(response.AccessToken != default(string));
            }
        }

        [Fact]
        public async Task ShouldGetAll()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetUsersQuery.Response>(Get.Users);

                Assert.True(response.Users.Count() > 0);
            }
        }
        
        [Fact]
        public async Task ShouldGetById()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetUserByIdQuery.Response>(Get.UserById(1));

                Assert.True(response.User.UserId != default(int));
            }
        }
        
        [Fact]
        public async Task ShouldUpdate()
        {
            using (var server = CreateServer())
            {
                var getByIdResponse = await server.CreateClient()
                    .GetAsync<GetUserByIdQuery.Response>(Get.UserById(1));

                Assert.True(getByIdResponse.User.UserId != default(int));

                var saveResponse = await server.CreateClient()
                    .PostAsAsync<SaveUserCommand.Request, SaveUserCommand.Response>(Post.Users, new SaveUserCommand.Request()
                    {
                        User = getByIdResponse.User
                    });

                Assert.True(saveResponse.UserId != default(int));
            }
        }
        
        [Fact]
        public async Task ShouldDelete()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .DeleteAsync(Delete.User(1));

                response.EnsureSuccessStatusCode();
            }
        }
    }
}
