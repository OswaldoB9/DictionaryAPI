namespace DictionaryAPI.DTOs_1
{
    public class WordResponseDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        //public string Definition { get; set; } = null!;
        public List<string> Definitions { get; set; } = new();
        public string PartOfSpeech { get; set; } = null!;
        public string? Example { get; set; }
        public string Language { get; set; } = null!; // "es"
        public List<string> Synonyms { get; set; } = new();
        public List<string> Translations { get; set; } = new();


    }
}
