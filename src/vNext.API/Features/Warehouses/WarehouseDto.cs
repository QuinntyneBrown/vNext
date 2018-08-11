using System;
using vNext.API.Features.Notes;
using vNext.Core.Models;

namespace vNext.API.Features.Warehouses
{
    public class WarehouseDto
    {        
        public int WarehouseId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedByUserId { get; set; }
        public float HandlingCharge { get; set; }
        public int AddressId { get; set; }
        public int NoteId { get; set; }
        public string Settings { get; set; } = "{}";
        public NoteDto Note { get; set; }
        public static WarehouseDto FromWarehouse(dynamic warehouse)
            => new WarehouseDto
            {
                WarehouseId = warehouse.WarehouseId,
                Code = warehouse.Code,
                Description = warehouse.Description,
                Status = warehouse.Status,
                CreatedDateTime = warehouse.CreatedDateTime,
                CreatedDate = warehouse.CreatedDate,
                CreatedByUserId = warehouse.CreatedByUserId,
                HandlingCharge = warehouse.HandlingCharge,
                AddressId = warehouse.AddressId,
                NoteId = warehouse.NoteId,
                Settings = warehouse.Settings,
                Note = new NoteDto()
                {
                    NoteId = warehouse.NoteId,
                    Note = warehouse.Note
                }
            };
    }
}
