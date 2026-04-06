namespace DictionaryAPI.DTOs_1
{
    public class SearchWordQueryDto
    {
        public string? Text { get; set; }
        public string? Language { get; set; }
        public string? PartOfSpeech { get; set; }

        public bool IncludeTranslations { get; set; } = false;
        public bool IncludeSynonyms { get; set; } = false;

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        public string OrderBy { get; set; } = "text";
    }
}
