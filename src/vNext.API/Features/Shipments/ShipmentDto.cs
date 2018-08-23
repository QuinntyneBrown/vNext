using vNext.Core.Models;

namespace vNext.API.Features.Shipments
{
    public class ShipmentDto
    {        
        public int ShipmentId { get; set; }
        public string Code { get; set; }

        public static ShipmentDto FromShipment(dynamic shipment)
            => new ShipmentDto
            {
                ShipmentId = shipment.ShipmentId,
                Code = shipment.Code
            };
    }
}
