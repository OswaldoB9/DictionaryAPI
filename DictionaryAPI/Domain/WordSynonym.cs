namespace DictionaryAPI.Domain
{
    public class WordSynonym
    {
        public int WordId { get; set; }
        public Word Word { get; set; } = null!;

        public int SynonymId { get; set; }
        public Word Synonym { get; set; } = null!;
    }
}
