namespace vNext.Core.Models
{
    public class CountrySubDivision
    {
        public int CountrySubDivisionId { get; set; }           
		public string Code { get; set; }      
        public int CountryId { get; set; }
        public string Description { get; set; }
    }
}
