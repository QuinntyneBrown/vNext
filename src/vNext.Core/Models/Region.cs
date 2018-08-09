using System;

namespace vNext.Core.Models
{
    public class Region
    {
        public short RegionId { get; set; }
        public string Code { get; set; }       
        public string Description { get; set; }
        public int Sort { get; set; }
        public int NoteId { get; set; }
        public string Note { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int ConcurrencyVersion { get; set; }
    }
}
