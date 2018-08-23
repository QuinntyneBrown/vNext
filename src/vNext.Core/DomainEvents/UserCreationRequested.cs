using MediatR;

namespace vNext.Core.DomainEvents
{
    public class UserCreationRequested: INotification
    {
        public UserCreationRequested(string code, string password)
        {
            Password = password;

            Code = code;
            Status = 0;
            Settings = "{}";
            ContactId = 4;
            DivisionId = 1;
            WarehouseId = 1;
            NoteId = 0;
        }

        public string Code { get; set; }
        public string Password { get; set; }
        public short Status { get; set; }
        public string Settings { get; set; }
        public int ContactId { get; set; }
        public int DivisionId { get; set; }
        public int WarehouseId { get; set; }
        public int NoteId { get; set; }
    }
}
