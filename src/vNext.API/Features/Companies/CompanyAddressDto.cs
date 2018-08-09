using vNext.Core.Models;

namespace vNext.CompanyService.CompanyAddresses
{
    public class CompanyAddressDto
    {        
        public int CompanyAddressId { get; set; }
        public string Code { get; set; }

        public static CompanyAddressDto FromCompanyAddress(CompanyAddress companyAddress)
            => new CompanyAddressDto
            {
                CompanyAddressId = companyAddress.CompanyAddressId,
                Code = companyAddress.Code
            };
    }
}
