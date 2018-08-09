using System;
using vNext.API.Features.Notes;

namespace vNext.API.Features.Users
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Code { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedByUserId { get; set; }
        public int ContactId { get; set; }
        public int DivisionId { get; set; }
        public int WarehouseId { get; set; }
        public string Settings { get; set; }
        public int NoteId { get; set; }
        public NoteDto Note { get; set; }

        public static UserDto FromUser(dynamic user)
            => new UserDto
            {
                UserId = user.UserId,
                Code = user.Code,
                Status = user.Status,
                CreatedByUserId = user.CreatedByUserId,
                CreatedDateTime = user.CreatedDateTime,
                ContactId = user.ContactId,
                DivisionId = user.DivisionId,
                WarehouseId = user.WarehouseId,
                Settings = user.Settings,
                NoteId = user.NoteId,
                Note = new NoteDto()
                {
                    NoteId = user.NoteId,
                    Note = user.Note
                }
            };
    }
}
