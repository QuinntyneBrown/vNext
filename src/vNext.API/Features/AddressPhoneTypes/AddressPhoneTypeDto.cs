using vNext.Core.Models;

namespace vNext.API.Features.AddressPhoneTypes
{
    public class AddressPhoneTypeDto
    {        
        public int AddressPhoneTypeId { get; set; }
        public string Description { get; set; }
        public static AddressPhoneTypeDto FromAddressPhoneType(AddressPhoneType addressPhoneType)
            => new AddressPhoneTypeDto
            {
                AddressPhoneTypeId = addressPhoneType.AddressPhoneTypeId,
                Description = addressPhoneType.Description
            };
    }
}
