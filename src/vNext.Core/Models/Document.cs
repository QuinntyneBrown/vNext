namespace vNext.Core.Models
{
    public class DocumentBase {
        public byte[] Document { get; set; }
    }
    public class Document: DocumentBase
    {
        public int DocumentId { get; set; }           		
    }
}
