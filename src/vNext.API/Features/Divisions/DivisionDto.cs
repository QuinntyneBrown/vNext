using vNext.API.Features.Notes;

namespace vNext.API.Features.Divisions
{
    public class DivisionDto
    {        
        public int DivisionId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public NoteDto Note { get; set; }
        public int AddressId { get; set; }
        public int RegionId { get; set; }
        public int Sort { get; set; }
        public int ConcurrencyVersion { get; set; }

        public static DivisionDto FromDivision(dynamic division)
            => new DivisionDto
            {
                DivisionId = division.DivisionId,
                Code = division.Code,
                Description = division.Description,
                Status = division.Status,
                AddressId = division.AddressId,
                RegionId = division.RegionId,
                Sort = division.Sort,
                ConcurrencyVersion = division.ConcurrencyVersion,
                Note = new NoteDto() { Note = division.Note }
            };
    }
}
