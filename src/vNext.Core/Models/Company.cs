using System;

namespace vNext.Core.Models
{
    public class Company
    {
        public int CompanyId { get; set; }           
		public string Code { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedByUserId { get; set; }
        public int AddressId { get; set; }
        public int CurrencyId { get; set; }
        public string Settings { get; set; }
        public int NoteId { get; set; }
        public int ConcurrencyVersion { get; set; }
    }
}
