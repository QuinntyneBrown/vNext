using vNext.Core.Models;

namespace vNext.API.Features.AddressEmailTypes
{
    public class AddressEmailTypeDto
    {        
        public int AddressEmailTypeId { get; set; }
        public string Description { get; set; }
        public static AddressEmailTypeDto FromAddressEmailType(AddressEmailType addressEmailType)
            => new AddressEmailTypeDto
            {
                AddressEmailTypeId = addressEmailType.AddressEmailTypeId,
                Description = addressEmailType.Description
            };
    }
}
