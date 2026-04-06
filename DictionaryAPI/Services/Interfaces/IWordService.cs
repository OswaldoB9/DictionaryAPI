using DictionaryAPI.Domain;
using DictionaryAPI.DTOs_1;
using DictionaryAPI.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DictionaryAPI.Services.Interfaces
{
    public interface IWordService
    {
        Task<List<Word>> GetAllAsync(string? text, string? languageCode);
        Task<Word?> GetByIdAsync(int id);
        Task<Word> CreateAsync(CreateWordDto dto);
        Task <Word> AddSynonymAsync(int wordId, int synonymId);
        Task<Word> AddTranslationAsync(int wordId, int translationId);
        Task<PagedResponseDto<WordSearchResponseDto>> SearchAsync(SearchWordQueryDto query);



    }

    public class WordService : IWordService
    {
        private readonly DictionaryDbContext _context;

        public WordService(DictionaryDbContext context)
        {
            _context = context;
        }

        public async Task<List<Word>> GetAllAsync(string? text, string? languageCode)
        {
            var query = _context.Words
                .Include(w => w.Language)
                .Include(w => w.PartOfSpeech)
                .Include(w => w.Definitions)
                .Include(w => w.Synonyms)
                    .ThenInclude(ws => ws.Synonym)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(text))
                query = query.Where(w => w.Text.Contains(text));

            if (!string.IsNullOrWhiteSpace(languageCode))
                query = query.Where(w => w.Language.Code == languageCode);

            return await query.ToListAsync();
        }

        public async Task<Word?> GetByIdAsync(int id)
        {
            return await _context.Words
               .Include(w => w.Language)
               .Include(w => w.PartOfSpeech)
               .Include(w => w.Definitions)
               .Include(w => w.Synonyms)
                   .ThenInclude(ws => ws.Synonym)
               .Include(w => w.SynonymOf)
                   .ThenInclude(ws => ws.Word)
               .Include(w => w.Translations)
                     .ThenInclude(wt => wt.Translation)
                     .ThenInclude(t => t.Language)
               .Include(w => w.TranslationOf)
                    .ThenInclude(wt => wt.Word)
                    .ThenInclude(t => t.Language)
               .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<Word> AddSynonymAsync(int wordId, int synonymId)
        {
            if (wordId == synonymId)
                throw new BadRequestException("Una palabra no puede ser sinónimo de sí misma");

            var word = await _context.Words
                .Include(w => w.Synonyms)
                .Include(w => w.SynonymOf)
                .FirstOrDefaultAsync(w => w.Id == wordId)
                ?? throw new NotFoundException($"Palabra {wordId} no existe");

            var synonym = await _context.Words
                .FirstOrDefaultAsync(w => w.Id == synonymId)
                ?? throw new NotFoundException($"Palabra {synonymId} no existe");

            var exists = await _context.WordSynonyms.AnyAsync(ws =>
                (ws.WordId == wordId && ws.SynonymId == synonymId) ||
                (ws.WordId == synonymId && ws.SynonymId == wordId));

            if (!exists)
            {
                _context.WordSynonyms.Add(new WordSynonym
                {
                    WordId = wordId,
                    SynonymId = synonymId
                });

                await _context.SaveChangesAsync();
            }

            return await _context.Words
                .Include(w => w.Language)
                .Include(w => w.PartOfSpeech)
                .Include(w => w.Definitions)
                .Include(w => w.Synonyms)
                    .ThenInclude(ws => ws.Synonym)
                .Include(w => w.SynonymOf)
                    .ThenInclude(ws => ws.Word)
                .FirstAsync(w => w.Id == wordId);
        }


        public async Task<Word> CreateAsync(CreateWordDto dto)
        {
            // 1️⃣ Buscar el idioma por código
            var language = await _context.Languages
                .FirstOrDefaultAsync(l => l.Code == dto.LanguageCode) ?? throw new Exception($"Idioma no soportado: {dto.LanguageCode}");

            var partOfSpeech = await _context.PartsOfSpeech
                .FirstOrDefaultAsync(p => p.Name == dto.PartOfSpeech) ?? throw new BadRequestException($"Parte del habla no válida: {dto.PartOfSpeech}");

            var word = new Word
            {
                Text = dto.Text,
                Example = dto.Example,
                LanguageId = language.Id,
                PartOfSpeechId = partOfSpeech.Id,
                Definitions = dto.Definitions.Select(d => new Definition { Text = d }).ToList()
            };

            _context.Words.Add(word);
            await _context.SaveChangesAsync();

            await _context.Entry(word).Reference(w => w.Language).LoadAsync();
            await _context.Entry(word).Reference(w => w.PartOfSpeech).LoadAsync();
            await _context.Entry(word).Collection(w => w.Definitions).LoadAsync();


            return word;
        }

        public async Task<Word> AddTranslationAsync(int wordId, int translationId)
        {
            if (wordId == translationId)
                throw new BadRequestException(
                    "Una palabra no puede traducirse a sí misma");

            var word = await _context.Words
                .Include(w => w.Language)
                .FirstOrDefaultAsync(w => w.Id == wordId)
                ?? throw new NotFoundException($"Palabra {wordId} no existe");

            var translation = await _context.Words
                .Include(w => w.Language)
                .FirstOrDefaultAsync(w => w.Id == translationId)
                ?? throw new NotFoundException($"Palabra {translationId} no existe");

            if (word.LanguageId == translation.LanguageId)
                throw new BadRequestException(
                    "Las traducciones deben ser entre idiomas distintos");

            var exists = await _context.WordTranslations.AnyAsync(wt =>
                (wt.WordId == wordId && wt.TranslationId == translationId) ||
                (wt.WordId == translationId && wt.TranslationId == wordId));

            if (!exists)
            {
                _context.WordTranslations.Add(new WordTranslation
                {
                    WordId = wordId,
                    TranslationId = translationId
                });

                await _context.SaveChangesAsync();
            }

            return await _context.Words
                .Include(w => w.Language)
                .Include(w => w.PartOfSpeech)
                .Include(w => w.Definitions)
                .Include(w => w.Translations)
                    .ThenInclude(wt => wt.Translation)
                        .ThenInclude(t => t.Language)
                .Include(w => w.TranslationOf)
                    .ThenInclude(wt => wt.Word)
                        .ThenInclude(t => t.Language)
                .FirstAsync(w => w.Id == wordId);
        }

        public async Task<PagedResponseDto<WordSearchResponseDto>> SearchAsync(SearchWordQueryDto query)
        {
            var wordsQuery = _context.Words
                .Include(w => w.Language)
                .Include(w => w.PartOfSpeech)
                .AsQueryable();

            // Texto
            if (!string.IsNullOrWhiteSpace(query.Text))
            {
                wordsQuery = wordsQuery.Where(w =>
                    w.Text.Contains(query.Text));
            }

            // Idioma
            if (!string.IsNullOrWhiteSpace(query.Language))
            {
                wordsQuery = wordsQuery.Where(w =>
                    w.Language.Code == query.Language);
            }

            // Parte del habla
            if (!string.IsNullOrWhiteSpace(query.PartOfSpeech))
            {
                wordsQuery = wordsQuery.Where(w =>
                    w.PartOfSpeech.Name == query.PartOfSpeech);
            }

            // Orden
            wordsQuery = query.OrderBy.ToLower() switch
            {
                "language" => wordsQuery.OrderBy(w => w.Language.Code),
                _ => wordsQuery.OrderBy(w => w.Text)
            };

            var totalItems = await wordsQuery.CountAsync();

            var items = await wordsQuery
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(w => new WordSearchResponseDto
                {
                    Id = w.Id,
                    Text = w.Text,
                    Language = w.Language.Code,
                    PartOfSpeech = w.PartOfSpeech.Name
                })
                .ToListAsync();

            return new PagedResponseDto<WordSearchResponseDto>
            {
                Items = items,
                Page = query.Page,
                PageSize = query.PageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)query.PageSize)
            };
        }


    }
    public class PagedResult<T>
    {
        public List<T> Items { get; }
        public int TotalItems { get; }
        public int Page { get; }
        public int PageSize { get; }
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);

        public PagedResult(List<T> items, int totalItems, int page, int pageSize)
        {
            Items = items;
            TotalItems = totalItems;
            Page = page;
            PageSize = pageSize;
        }
    }

}
