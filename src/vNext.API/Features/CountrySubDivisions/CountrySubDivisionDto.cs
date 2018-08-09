using vNext.Core.Models;

namespace vNext.API.Features.CountrySubDivisions
{
    public class CountrySubDivisionDto
    {        
        public int CountrySubDivisionId { get; set; }
        public string Code { get; set; }
        public int CountryId { get; set; }
        public string Description { get; set; }

        public static CountrySubDivisionDto FromCountrySubDivision(CountrySubDivision countrySubDivision)
            => new CountrySubDivisionDto
            {
                CountrySubDivisionId = countrySubDivision.CountrySubDivisionId,
                Code = countrySubDivision.Code,
                CountryId = countrySubDivision.CountryId,
                Description = countrySubDivision.Description
            };
    }
}
