using System;
using System.Collections.Generic;

namespace Domain
{
    public class Phrase :BaseModel
    {
        public string Word { get; set; }
        public Guid LanguageId { get; set; }
        public Language Language { get; set; }
        public ICollection<UserPhrase> UserPhrases { get; set; }
        public ICollection<Translation> Translations { get; set; }

    }
}