using vNext.Core.Models;

namespace vNext.API.Features.Warehouses
{
    public class WarehouseDto
    {        
        public int WarehouseId { get; set; }
        public string Code { get; set; }

        public static WarehouseDto FromWarehouse(Warehouse warehouse)
            => new WarehouseDto
            {
                WarehouseId = warehouse.WarehouseId,
                Code = warehouse.Code
            };
    }
}
