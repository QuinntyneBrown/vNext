using System;

namespace vNext.Core.Models
{
    public class Manufacturer
    {
        public int ManufacturerId { get; set; }           
		public string Code { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedByUserId { get; set; }
        public int AddressId { get; set; }
        public string Settings { get; set; }
        public int ConcurrencyVersion { get; set; }
        public Address Address { get; set; }
        public User CreatedByUser { get; set; }
    }
}
