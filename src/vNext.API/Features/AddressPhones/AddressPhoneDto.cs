using vNext.Core.Models;

namespace vNext.API.Features.AddressPhones
{
    public class AddressPhoneDto
    {        
        public int AddressPhoneId { get; set; }
        public string Phone { get; set; }
        public int Sort { get; set; }
        public int AddressPhoneTypeId { get; set; }
        public static AddressPhoneDto FromAddressPhone(AddressPhone addressPhone)
            => new AddressPhoneDto
            {
                AddressPhoneId = addressPhone.AddressPhoneId,
                Phone = addressPhone.Phone,
                Sort = addressPhone.Sort,
                AddressPhoneTypeId = addressPhone.AddressPhoneTypeId
            };
    }
}
