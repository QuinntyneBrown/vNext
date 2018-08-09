using System;
using vNext.API.Features.Notes;
using vNext.Core.Models;

namespace vNext.API.Features.Regions
{
    public class RegionDto
    {        
        public int RegionId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedByUserId { get; set; }
        public int Sort { get; set; }
        public int ConcurrencyVersion { get; set; }
        public int NoteId { get; set; }
        public NoteDto Note { get; set; } = new NoteDto();

        public static RegionDto FromRegion(dynamic region)
            => new RegionDto
            {
                RegionId = region.RegionId,
                Code = region.Code,
                Description = region.Description,
                CreatedDateTime = region.CreatedDateTime,
                CreatedByUserId = region.CreatedByUserId,
                Sort = region.Sort,
                NoteId = region.NoteId,
                Note = new NoteDto() { NoteId = region.NoteId, Note = region.Note },
                ConcurrencyVersion = region.ConcurrencyVersion
            };
    }
}
