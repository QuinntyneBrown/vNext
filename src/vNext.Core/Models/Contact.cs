using System;
using System.Collections.Generic;

namespace vNext.Core.Models
{
    public class Contact
    {
        public int ContactId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedByUserId { get; set; }
        public int AddressId { get; set; }
        public int? DocumentId { get; set; }
        public int NoteId { get; set; }
        public int ConcurrencyVersion { get; set; }
        public ICollection<ContactAddress> ContactAddresses { get; set; } = new HashSet<ContactAddress>();
    }
}
