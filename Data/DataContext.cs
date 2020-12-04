using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class DataContext:IdentityDbContext<User>
    {
        public DataContext(DbContextOptions opt) : base(opt) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserPhrase>()
                .HasKey(bc => new {bc.UserId, bc.PhaseId});
            builder.Entity<UserPhrase>()
                .HasOne(bc => bc.User)
                .WithMany(b => b.UserPhrases)
                .HasForeignKey(bc => bc.UserId);
            builder.Entity<UserPhrase>()
                .HasOne(bc => bc.Phrase)
                .WithMany(c => c.UserPhrases)
                .HasForeignKey(bc => bc.PhaseId);
        }

        public DbSet<Language> Languages { get; set; }
        public DbSet<Phrase> Phrases { get; set; }
        public DbSet<Translation> Translations { get; set; }

        public DbSet<UserPhrase> UserPhrases { get; set; }
        
    }
}