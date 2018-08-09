namespace vNext.Core.Models
{
    public class ContactAddress
    {
        public int ContactAddressId { get; set; }
        public int ContactId { get; set; }
        public int AddressId { get; set; }
        public int AddressTypeId { get; set; }
        public int Sort { get; set; }       
        public Address Address { get; set; }
        public AddressType AddressType { get; set; }
    }
}
