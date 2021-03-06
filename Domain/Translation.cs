﻿using System;

namespace Domain
{
    public class Translation:BaseModel
    {
        public Guid PhraseId { get; set; }
        public Phrase Phrase { get; set; }
        public Guid LanguageId { get; set; }
        public Language Language { get; set; }
        public string Word { get; set; }
    }
}