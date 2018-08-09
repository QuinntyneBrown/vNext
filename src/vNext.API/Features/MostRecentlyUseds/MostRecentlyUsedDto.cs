using vNext.Core.Models;

namespace vNext.API.Features.MostRecentlyUseds
{
    public class MostRecentlyUsedDto
    {        
        public int MostRecentlyUsedId { get; set; }
        public string Code { get; set; }

        public static MostRecentlyUsedDto FromMostRecentlyUsed(dynamic mostRecentlyUsed)
            => new MostRecentlyUsedDto
            {
                MostRecentlyUsedId = mostRecentlyUsed.MostRecentlyUsedId,
                Code = mostRecentlyUsed.Code
            };
    }
}
