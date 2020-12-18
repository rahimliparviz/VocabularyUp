using System;
using System.Collections.Generic;

namespace Services.DTO
{
    public class PhrasesWithTranslationsDto
    {
        public Guid Id { get; set; }
        public string Word { get; set; }
        public Guid LanguageId { get; set; }
        public IEnumerable<TranslationPostDto> Translations { get; set; }
        
    }
}