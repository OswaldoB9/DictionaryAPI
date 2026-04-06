namespace DictionaryAPI.Domain
{
    public class Definition
    {
        public int Id { get; set; }

        public int WordId { get; set; }
        public Word Word { get; set; } = null!;

        public string Text { get; set; } = null!;
    }
}
