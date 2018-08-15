using vNext.API.Features.AddressEmails;
using vNext.API.Features.AddressPhones;
using vNext.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace vNext.API.Features.Addresses
{
    public class AddressDto
    {
        public int AddressId { get; set; }
        public int AddressBaseId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalZipCode { get; set; }
        public string County { get; set; }
        public int CountrySubdivisionId { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }        
        public string Phone { get; set; }
        public string Website { get; set; }
        public IEnumerable<AddressEmailDto> AddressEmails { get; set; } = new HashSet<AddressEmailDto>();
        public IEnumerable<AddressPhoneDto> AddressPhones { get; set; } = new HashSet<AddressPhoneDto>();

        public static AddressDto FromAddress(Address address)
            => new AddressDto
            {
                AddressId = address.AddressId,
                Address = address.Address,
                Email = address.Email,
                Fax = address.Fax,
                Phone = address.Phone,
                Website = address.Website,
                County = address.County,
                CountrySubdivisionId = address.CountrySubdivisionId,
                AddressEmails = address.AddressEmails.Select(x => AddressEmailDto.FromAddressEmail(x)).ToList(),
                AddressPhones = address.AddressPhones.Select(x => AddressPhoneDto.FromAddressPhone(x)).ToList()
            };
    }
}
