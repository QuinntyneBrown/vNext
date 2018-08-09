using vNext.Core.Models;

namespace vNext.API.Features.Countries
{
    public class CountryDto
    {        
        public int CountryId { get; set; }
        public string Code2 { get; set; }
        public string Code3 { get; set; }
        public string Description { get; set; }
        public string NumericCode { get; set; }

        public static CountryDto FromCountry(Country country)
            => new CountryDto
            {
                CountryId = country.CountryId,
                Code2 = country.Code2,
                Code3 = country.Code3,
                NumericCode = country.NumericCode,
                Description = country.Description
            };
    }
}
