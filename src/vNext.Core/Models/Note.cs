namespace vNext.Core.Models
{
    public class NoteBase
    {
        public int NoteBaseId { get; set; }
        public string Note { get; set; }
    }

    public class Note: NoteBase
    {
        public int NoteId { get; set; }          
    }
}
