using DictionaryAPI.Domain;
using Microsoft.EntityFrameworkCore;

namespace DictionaryAPI.Services
{
    public class DictionaryDbContext : DbContext
    {
        public DictionaryDbContext(DbContextOptions<DictionaryDbContext> options)
            : base(options)
        {
        }

        public DbSet<Word> Words => Set<Word>();
        public DbSet<Language> Languages => Set<Language>();
        public DbSet<Definition> Definitions => Set<Definition>();
        public DbSet<PartOfSpeech> PartsOfSpeech => Set<PartOfSpeech>();
        public DbSet<WordSynonym> WordSynonyms => Set<WordSynonym>();
        public DbSet<WordTranslation> WordTranslations => Set<WordTranslation>();





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Language>()
                .HasIndex(l => l.Code)
                .IsUnique();

            modelBuilder.Entity<Word>()
                .HasOne(w => w.Language)
                .WithMany()
                .HasForeignKey(w => w.LanguageId);

            modelBuilder.Entity<Definition>()
                .HasOne(d => d.Word)
                .WithMany(w => w.Definitions)
                .HasForeignKey(d => d.WordId);

            modelBuilder.Entity<PartOfSpeech>()
                .HasIndex(p => p.Name)
                .IsUnique();

            modelBuilder.Entity<WordSynonym>()
                .HasKey(ws => new { ws.WordId, ws.SynonymId });
            
            modelBuilder.Entity<WordSynonym>()
                .HasOne(ws => ws.Word)
                .WithMany(w => w.Synonyms)
                .HasForeignKey(ws => ws.WordId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WordSynonym>()
                .HasOne(ws => ws.Synonym)
                .WithMany(w => w.SynonymOf)
                .HasForeignKey(ws => ws.SynonymId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<WordTranslation>()
                .HasKey(wt => new { wt.WordId, wt.TranslationId });

            modelBuilder.Entity<Language>().HasData(
                new Language { Id = 1, Code = "es", Name = "Español" },
                new Language { Id = 2, Code = "en", Name = "English" },
                new Language { Id = 3, Code = "fr", Name = "Français" },
                new Language { Id = 4, Code = "de", Name = "Alemán" }
             );

            modelBuilder.Entity<PartOfSpeech>().HasData(
                new PartOfSpeech { Id = 1, Name = "Sustantivo" },
                new PartOfSpeech { Id = 2, Name = "Verbo" },
                new PartOfSpeech { Id = 3, Name = "Adjetivo" },
                new PartOfSpeech { Id = 4, Name = "Adverbio" }
             );


            modelBuilder.Entity<WordTranslation>()
                .HasOne(wt => wt.Word)
                .WithMany(w => w.Translations)
                .HasForeignKey(wt => wt.WordId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WordTranslation>()
                .HasOne(wt => wt.Translation)
                .WithMany(w => w.TranslationOf)
                .HasForeignKey(wt => wt.TranslationId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
