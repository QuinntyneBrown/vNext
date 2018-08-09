using vNext.Core.Models;

namespace vNext.API.Features.Territories
{
    public class TerritoryDto
    {        
        public int TerritoryId { get; set; }
        public string Code { get; set; }

        public static TerritoryDto FromTerritory(dynamic territory)
            => new TerritoryDto
            {
                TerritoryId = territory.TerritoryId,
                Code = territory.Code
            };
    }
}
