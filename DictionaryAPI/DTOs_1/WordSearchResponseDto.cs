namespace DictionaryAPI.DTOs_1
{
    public class WordSearchResponseDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = default!;
        public string Language { get; set; } = default!;
        public string PartOfSpeech { get; set; } = default!;
    }
}
