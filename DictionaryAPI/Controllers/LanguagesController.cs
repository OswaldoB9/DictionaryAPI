namespace DictionaryAPI.Controllers
{
    using DictionaryAPI.DTOs_1;
    using DictionaryAPI.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/languages")]
    public class LanguagesController : ControllerBase
    {
        private readonly ILanguageService _service;

        public LanguagesController(ILanguageService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _service.GetAllAsync()
                .Result
                .Select(l => new LanguageDto
                {
                    Code = l.Code,
                    Name = l.Name
                });

            return Ok(result);
        }
    }

}
