using System;

namespace Domain
{
    public class Translation:BaseModel
    {
        public Guid PhraseId { get; set; }
        public Phrase Phrase { get; set; }
        public string Word { get; set; }
    }
}