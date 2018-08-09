using vNext.Core.Models;

namespace vNext.API.Features.Sizes
{
    public class SizeDto
    {        
        public int SizeId { get; set; }
        public string Code { get; set; }

        public static SizeDto FromSize(dynamic size)
            => new SizeDto
            {
                SizeId = size.SizeId,
                Code = size.Code
            };
    }
}
