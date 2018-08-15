namespace vNext.API.Features.Notes
{
    public class NoteDto
    {        
        public int NoteId { get; set; }
        public string Note { get; set; }
        public static NoteDto FromNote(dynamic note)
            => new NoteDto
            {
                NoteId = note.NoteId,
                Note = note.Note
            };
    }
}
