using vNext.Core.Models;

namespace vNext.API.Features.ShipTos
{
    public class ShipToDto
    {        
        public int ShipToId { get; set; }
        public string Code { get; set; }

        public static ShipToDto FromShipTo(dynamic shipTo)
            => new ShipToDto
            {
                ShipToId = shipTo.ShipToId,
                Code = shipTo.Code
            };
    }
}
