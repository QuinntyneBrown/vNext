using vNext.Core.Models;

namespace vNext.API.Features.Discounts
{
    public class DiscountDto
    {        
        public int DiscountId { get; set; }
        public string Code { get; set; }

        public static DiscountDto FromDiscount(dynamic discount)
            => new DiscountDto
            {
                DiscountId = discount.DiscountId,
                Code = discount.Code
            };
    }
}
