using System;

namespace vNext.Core.Models
{
    public class MostRecentlyUsed
    {
        public int MostRecentlyUsedId { get; set; }           
		public int UserId { get; set; }
        public string Domain { get; set; }
        public int Id { get; set; }
        public DateTime MostRecentlyUsedDateTime { get; set; }
        public DateTime MostRecentlyUsedDate { get; set; }
        public User User { get; set; }
    }
}
