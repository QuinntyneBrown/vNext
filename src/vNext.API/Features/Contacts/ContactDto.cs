using vNext.Core.Models;
using System;

namespace vNext.API.Features.Contacts
{
    public class ContactDto
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

        public static ContactDto FromContact(dynamic contact)
            => new ContactDto
            {
                ContactId = contact.ContactId,
                FirstName = contact.FirstName,
                MiddleName = contact.MiddleName,
                LastName = contact.LastName,
                CompanyName = contact.CompanyName,
                CreatedDateTime = contact.CreatedDateTime,
                CreatedDate = contact.CreatedDate,
                CreatedByUserId = contact.CreatedByUserId,
                AddressId = contact.AddressId,
                DocumentId = contact.DocumentId,
                NoteId = contact.NoteId,
                ConcurrencyVersion = contact.ConcurrencyVersion
            };
    }
}
