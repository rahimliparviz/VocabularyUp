﻿using System;

namespace Services.DTO
{
    public class PhrasesWithTranslationDto
    {
        public Guid PhraseId { get; set; }
        public string Phrase { get; set; }
        public string Translation { get; set; }
        public int NumberOfRemainingRepetitions { get; set; }
    }
}