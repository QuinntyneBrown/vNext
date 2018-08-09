namespace vNext.API.Features.Notes
{
    public class NoteDto
    {        
        public int NoteId { get; set; }
        public string Note { get; set; }
        public static NoteDto FromNote(vNext.Core.Models.Note note)
            => new NoteDto
            {
                NoteId = note.NoteId,
                Note = note.Note
            };
    }
}
