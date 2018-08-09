using vNext.CompanyService.CompanyAddresses;
using vNext.Core.Models;
using System;
using System.Collections.Generic;

namespace vNext.API.Features.Companies
{
    public class CompanyDto
    {
        public int CompanyId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedByUserId { get; set; }
        public int AddressId { get; set; }
        public int CurrencyId { get; set; }
        public string Settings { get; set; }
        public int NoteId { get; set; }
        public int ConcurrencyVersion { get; set; }
        public ICollection<CompanyAddressDto> CompanyAddresses { get; set; } = new HashSet<CompanyAddressDto>();

        public static CompanyDto FromCompany(Company company)
            => new CompanyDto
            {
                CompanyId = company.CompanyId,
                Code = company.Code
            };
    }
}
