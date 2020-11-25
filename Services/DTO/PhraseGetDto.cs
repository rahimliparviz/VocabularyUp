using System;

namespace Services.DTO
{
    public class PhraseGetDto
    {
        public Guid Id { get; set; }
        public string Word { get; set; }
        public Guid LanguageId { get; set; }
    }
}