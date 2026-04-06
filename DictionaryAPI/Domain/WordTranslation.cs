namespace DictionaryAPI.Domain
{
    public class WordTranslation
    {
        public int WordId { get; set; }
        public Word Word { get; set; } = null!;

        public int TranslationId { get; set; }
        public Word Translation { get; set; } = null!;
    }
}
