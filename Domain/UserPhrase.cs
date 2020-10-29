using System;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class UserPhrase
    {
        public Guid PhaseId { get; set; }
        public Phrase Phrase { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}