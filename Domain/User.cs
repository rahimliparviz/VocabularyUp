using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class User:IdentityUser
    {
        public ICollection<UserPhrase> UserPhrases { get; set; }

    }
}