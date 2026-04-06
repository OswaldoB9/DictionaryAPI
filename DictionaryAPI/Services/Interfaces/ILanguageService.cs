using DictionaryAPI.Domain;

namespace DictionaryAPI.Services.Interfaces
{
    public interface ILanguageService
    {
        Task<List<Language>> GetAllAsync();
    }
}
