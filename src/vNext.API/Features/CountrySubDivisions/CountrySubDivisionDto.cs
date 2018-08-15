using vNext.Core.Models;

namespace vNext.API.Features.CountrySubdivisions
{
    public class CountrySubdivisionDto
    {        
        public int CountrySubdivisionId { get; set; }
        public string Code { get; set; }
        public int CountryId { get; set; }
        public string Description { get; set; }

        public static CountrySubdivisionDto FromCountrySubdivision(CountrySubdivision countrySubdivision)
            => new CountrySubdivisionDto
            {
                CountrySubdivisionId = countrySubdivision.CountrySubdivisionId,
                Code = countrySubdivision.Code,
                CountryId = countrySubdivision.CountryId,
                Description = countrySubdivision.Description
            };
    }
}
