using System.Collections.Generic;

namespace vNext.Core.Models
{
    public class Address: AddressBase
    {
        public int AddressId { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public ICollection<AddressEmail> AddressEmails { get; set; } = new HashSet<AddressEmail>();
        public ICollection<AddressPhone> AddressPhones { get; set; } = new HashSet<AddressPhone>();
    }

    public class AddressBase
    {
        public int AddressBaseId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalZipCode { get; set; }
        public string County { get; set; }
        public int CountrySubDivisionId { get; set; }
    }
}
