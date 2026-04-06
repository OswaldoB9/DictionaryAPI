using System.ComponentModel.DataAnnotations;

namespace DictionaryAPI.DTOs_1
{
    public class CreateWordDto
    {
        [Required]
        [StringLength(100)]
        public string Text { get; set; } = null!;
        //public string Language { get; set; } = null!;
        public string LanguageCode { get; set; } = null!; // "es", "en"
                                                          //public string Definition { get; set; } = null!;
        public string PartOfSpeech { get; set; } = null!; // "Verbo"

        public List<string> Definitions { get; set; } = new();
        public string? Example { get; set; }
    }
}
