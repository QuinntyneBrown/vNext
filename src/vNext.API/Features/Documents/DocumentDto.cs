using vNext.Core.Models;

namespace vNext.API.Features.Documents
{
    public class DocumentDto
    {        
        public int DocumentId { get; set; }
        public byte[] Document { get; set; }

        public static DocumentDto FromDocument(dynamic document)
            => new DocumentDto
            {
                DocumentId = document.DocumentId,
                Document = document.Document
            };
    }
}
