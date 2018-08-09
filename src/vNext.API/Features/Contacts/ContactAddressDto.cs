using vNext.Core.Models;

namespace vNext.ContactService.ContactAddresses
{
    public class ContactAddressDto
    {        
        public int ContactAddressId { get; set; }
        public string Code { get; set; }

        public static ContactAddressDto FromContactAddress(ContactAddress contactAddress)
            => new ContactAddressDto
            {
                ContactAddressId = contactAddress.ContactAddressId
            };
    }
}
