using System;

namespace Services.DTO
{
    public class TranslationGetDto
    {
        public Guid Id { get; set; }
        public Guid PhraseId { get; set; }
        public Guid LanguageId { get; set; }
        public string Word { get; set; }
    }
}