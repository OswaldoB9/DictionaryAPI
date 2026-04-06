namespace DictionaryAPI.Domain
{
    public class Language
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!; // es, en
        public string Name { get; set; } = null!; // Español, English
    }

}
