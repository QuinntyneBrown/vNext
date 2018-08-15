using System.Threading.Tasks;
using System.Collections.Generic;
using Xunit;
using vNext.API.Features.Addresses;
using vNext.Core.Extensions;
using vNext.API.Features.AddressPhones;
using vNext.API.Features.AddressEmails;

namespace IntegrationTests.Features
{
    public class AddressScenarios: AddressScenarioBase
    {
        [Fact]
        public async Task ShouldGetAddressById()
        {
            using (var server = CreateServer())
            {
                var addressId = 72;
                var response = await server.CreateClient()
                    .GetAsync<GetAddressByIdQuery.Response>(Get.AddressById(addressId));

                Assert.True(response.Address.AddressId == addressId);
            }
        }

        [Fact]
        public async Task ShouldSaveAddress()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .PostAsAsync<SaveAddressCommand.Request, SaveAddressCommand.Response>(Post.Addresses,new SaveAddressCommand.Request() {
                        Address = new AddressDto()
                        {
                            Address = "17000 Yonge st.",
                            City = "Toronto",
                            PostalZipCode = "N1M 1M1",
                            County = "Canda",
                            CountrySubdivisionId = 1,
                            Phone = "4161551234",
                            Fax = "4161116222",
                            Email = "noreply6@comsenseinc.com",
                            Website = "http://www2.comsenseinc.com",
                            AddressEmails = new List<AddressEmailDto>() {
                                new AddressEmailDto()
                                {
                                   AddressEmailTypeId = 1,
                                   Email = "quinntynebrown@gmail.com"
                                }
                            },
                            AddressPhones = new List<AddressPhoneDto>() {
                                new AddressPhoneDto()
                                {
                                    AddressPhoneTypeId = 1,
                                    Phone = "4169218181"
                                }
                            }
                        }
                    });

                Assert.True(response.AddressId == 72);
            }
        }

        [Fact]
        public async Task ShouldSaveMinimalAddress()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .PostAsAsync<SaveAddressCommand.Request, SaveAddressCommand.Response>(Post.Addresses, new SaveAddressCommand.Request()
                    {
                        Address = new AddressDto()
                        {
                            Address = "",
                            City = "",
                            PostalZipCode = "",
                            County = "",
                            CountrySubdivisionId = 1,
                            Phone = "",
                            Fax = "",
                            Email = "",
                            Website = ""
                        }
                    });

                Assert.True(response.AddressId != default(int));
            }
        }
    }
}
