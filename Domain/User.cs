using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class User:IdentityUser
    {
        public string Role { get; set; }
        public ICollection<UserPhrase> UserPhrases { get; set; }

    }
}