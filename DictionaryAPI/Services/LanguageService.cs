using DictionaryAPI.Domain;
using DictionaryAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DictionaryAPI.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly DictionaryDbContext _context;

        public LanguageService(DictionaryDbContext context)
        {
            _context = context;
        }

        public async Task<List<Language>> GetAllAsync()
        {
            return await _context.Languages
                .OrderBy(l => l.Name)
                .ToListAsync();
        }
    }
}
