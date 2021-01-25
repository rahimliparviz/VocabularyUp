using System;

namespace Services.DTO
{
    public class PhraseCartDto
    {
        public Guid PhraseId { get; set; }
        public string Phrase { get; set; }
        public string Translation { get; set; }
    }
}