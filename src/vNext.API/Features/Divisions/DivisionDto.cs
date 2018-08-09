namespace vNext.API.Features.Divisions
{
    public class DivisionDto
    {        
        public int DivisionId { get; set; }
        public string Code { get; set; }

        public static DivisionDto FromDivision(dynamic division)
            => new DivisionDto
            {
                DivisionId = division.DivisionId,
                Code = division.Code
            };
    }
}
