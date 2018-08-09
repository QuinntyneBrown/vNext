using System.Threading.Tasks;
using vNext.API.Features.Contacts;
using Xunit;
using vNext.Core.Extensions;
using System.Linq;
using System.Collections.Generic;

namespace IntegrationTests.Features
{
    public class ContactScenarios: ContactScenarioBase
    {
        [Fact]
        public async Task ShouldGetContactById()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetContactByIdQuery.Response>(Get.ContactById(1));

                Assert.True(response.Contact.ContactId == 1);
            }
        }

        [Fact]
        public async Task ShouldGetContacts()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<ContactGetAllQuery.Response>(Get.Contacts);

                Assert.True(response.Contacts.Count() > 0);
            }
        }

        [Fact]
        public async Task ShouldSaveContact()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .PostAsAsync<SaveContactCommand.Request, SaveContactCommand.Response>(Post.Contacts,new SaveContactCommand.Request() {
                        Contact = new ContactDto()
                        {
                            FirstName = "Quinntyne",
                            MiddleName = "Kale",
                            LastName = "Brown",
                            AddressId = 9,
                            CompanyName = "Comsense",
                            CreatedByUserId = 1
                        }
                    });

                Assert.True(response.ContactId == 3013);
            }
        }

        [Fact]
        public async Task ShouldDeleteContact()
        {
            using (var server = CreateServer())
            {
                var taskList = new List<Task>();

                foreach(var i in new List<int>() { 8,12,15,16,17,18 })
                {
                    taskList.Add(server.CreateClient()
                    .DeleteAsync<RemoveContactCommand.Response>(Delete.Contact(i, 0)));
                }

                Task.WaitAll(taskList.ToArray());

                Assert.True(true);
            }
        }
    }
}
