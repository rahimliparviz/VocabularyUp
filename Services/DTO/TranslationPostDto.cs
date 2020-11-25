using System;

namespace Services.DTO
{
    public class TranslationPostDto
    {
        public Guid LanguageId { get; set; }
        public string Word { get; set; }
    }
}