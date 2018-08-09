using vNext.Core.Models;

namespace vNext.API.Features.AddressEmails
{
    public class AddressEmailDto
    {        
        public int AddressEmailId { get; set; }
        public string Email { get; set; }
        public int AddressEmailTypeId { get; set; }
        public int Sort { get; set; }

        public static AddressEmailDto FromAddressEmail(AddressEmail addressEmail)
            => new AddressEmailDto
            {
                AddressEmailId = addressEmail.AddressEmailId,
                Email = addressEmail.Email,
                AddressEmailTypeId = addressEmail.AddressEmailTypeId,
                Sort = addressEmail.Sort
            };
    }
}
