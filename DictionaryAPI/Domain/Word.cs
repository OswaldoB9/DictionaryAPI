namespace DictionaryAPI.Domain
{
    public class Word
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public List<Definition> Definitions { get; set; } = new();
        public string? Example { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int PartOfSpeechId { get; set; }
        public PartOfSpeech PartOfSpeech { get; set; } = null!;

        public int LanguageId { get; set; }      // FK
        public Language Language { get; set; } = null!;

        public List<WordSynonym> Synonyms { get; set; } = new();
        public List<WordSynonym> SynonymOf { get; set; } = new();

        public List<WordTranslation> Translations { get; set; } = new();
        public List<WordTranslation> TranslationOf { get; set; } = new();
    }
}
