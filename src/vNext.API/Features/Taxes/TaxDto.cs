using vNext.Core.Models;

namespace vNext.API.Features.Taxes
{
    public class TaxDto
    {        
        public int TaxId { get; set; }
        public string Code { get; set; }

        public static TaxDto FromTax(dynamic tax)
            => new TaxDto
            {
                TaxId = tax.TaxId,
                Code = tax.Code
            };
    }
}
