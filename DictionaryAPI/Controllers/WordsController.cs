using DictionaryAPI.Domain;
using DictionaryAPI.DTOs_1;
using DictionaryAPI.Exceptions;
using DictionaryAPI.Services;
using DictionaryAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DictionaryAPI.Controllers
{
    [ApiController]
    [Route("api/words")]
    public class WordsController : ControllerBase
    {
        private readonly IWordService _service;

        public WordsController(IWordService service)
        {
            _service = service;
        }

        //[HttpGet("paged")]
        //public async Task<IActionResult> GetPaged(
        //    string? text,
        //    int page = 1,
        //    int pageSize = 10)
        //{
        //    return Ok(await _service.GetPagedAsync(text, page, pageSize));
        //}
        [HttpGet]
        public async Task<IActionResult> Get(string? text, string? language)
        {
            var words = await _service.GetAllAsync(text, language);

            var result = words.Select(w => new WordResponseDto
            {
                Id = w.Id,
                Text = w.Text,
                PartOfSpeech = w.PartOfSpeech.Name,
                Definitions = w.Definitions
                    .Select(d => d.Text)
                    .ToList(),
                Example = w.Example,
                Language = w.Language.Code,
                Synonyms = w.Synonyms
                .Select(s => s.Synonym.Text)
                .ToList()
            });

            return Ok(result);
        }
            //=> Ok(await _service.GetAllAsync(text, language));
        

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var word = await _service.GetByIdAsync(id) ?? throw new NotFoundException($"Palabra con id {id} no existe");
            var result = new WordResponseDto
            {
                Id = word.Id,
                Text = word.Text,
                PartOfSpeech = word.PartOfSpeech.Name,
                Definitions = word.Definitions
                    .Select(d => d.Text)
                    .ToList(),
                Example = word.Example,
                Language = word.Language.Code,
                Synonyms = word.Synonyms
                    .Select(s => s.Synonym.Text)
                    .Concat(
                        word.SynonymOf.Select(s => s.Word.Text)
                    )
                    .Distinct()
                    .ToList(),
                Translations = word.Translations
                    .Select(t => $"{t.Translation.Text} ({t.Translation.Language.Code})")
                    .Concat(
                        word.TranslationOf.Select(t =>
                            $"{t.Word.Text} ({t.Word.Language.Code})")
                    )
                    .Distinct()
                    .ToList()
            };
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWordDto dto)
        {
            //var word = await _service.CreateAsync(dto);
            //return CreatedAtAction(nameof(GetById), new { id = word.Id }, word);

            var word = await _service.CreateAsync(dto);

            var response = new WordResponseDto
            {
                Id = word.Id,
                Text = word.Text,
                Language = word.Language.Code,
                PartOfSpeech = word.PartOfSpeech.Name,
                Example = word.Example,
                Definitions = word.Definitions
                    .Select(d => d.Text)
                    .ToList()
            };

            return CreatedAtAction(nameof(GetById), new { id = word.Id }, response);
        }

        [HttpPost("{id}/synonyms/{synonymId}")]
        public async Task<IActionResult> AddSynonym(int id, int synonymId)
        {
            var word = await _service.AddSynonymAsync(id, synonymId);

            var result = new WordResponseDto
            {
                Id = word.Id,
                Text = word.Text,
                Language = word.Language.Code,
                PartOfSpeech = word.PartOfSpeech.Name,
                Example = word.Example,
                Definitions = word.Definitions.Select(d => d.Text).ToList(),
                Synonyms = word.Synonyms
                    .Select(s => s.Synonym.Text)
                    .Concat(word.SynonymOf.Select(s => s.Word.Text))
                    .Distinct()
                    .ToList()
            };

            return Ok(result);
        }

        [HttpPost("{id}/translations/{translationId}")]
        public async Task<IActionResult> AddTranslation(
    int id,
    int translationId)
        {
            var word = await _service.AddTranslationAsync(id, translationId);

            var result = new WordResponseDto
            {
                Id = word.Id,
                Text = word.Text,
                Language = word.Language.Code,
                PartOfSpeech = word.PartOfSpeech.Name,
                Definitions = word.Definitions.Select(d => d.Text).ToList(),
                Example = word.Example,
                Synonyms = word.Synonyms
                    .Select(s => s.Synonym.Text)
                    .Concat(word.SynonymOf.Select(s => s.Word.Text))
                    .Distinct()
                    .ToList(),
                Translations = word.Translations
                    .Select(t => $"{t.Translation.Text} ({t.Translation.Language.Code})")
                    .Concat(
                        word.TranslationOf.Select(t =>
                            $"{t.Word.Text} ({t.Word.Language.Code})")
                    )
                    .Distinct()
                    .ToList()
            };

            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] SearchWordQueryDto query)
        {
            var result = await _service.SearchAsync(query);
            return Ok(result);
        }



    }


}
