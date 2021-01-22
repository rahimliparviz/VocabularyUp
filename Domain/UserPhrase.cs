using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class UserPhrase
    {
        public Guid PhaseId { get; set; }
        public Phrase Phrase { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public Guid FromLanguageId { get; set; }
        public Guid ToLanguageId { get; set; }
        public int NumberOfRemainingRepetitions { get; set; }
        public DateTime LastSeen { get; set; }
    }
}