namespace vNext.Core.Models
{
    public class CountrySubdivision
    {
        public int CountrySubdivisionId { get; set; }           
		public string Code { get; set; }      
        public int CountryId { get; set; }
        public string Description { get; set; }
    }
}
