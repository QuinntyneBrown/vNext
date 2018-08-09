using System;

namespace vNext.Core.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Code { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedByUserId { get; set; }
        public int ContactId { get; set; }
        public int DivisionId { get; set; }
        public int WarehouseId { get; set; }
        public string Settings { get; set; }
        public int NoteId { get; set; }
        public string Note { get; set; }
    }
}
