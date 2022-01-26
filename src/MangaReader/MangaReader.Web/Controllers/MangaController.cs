using Microsoft.AspNetCore.Mvc;

namespace MangaReader.Web.Controllers
{
	using Core;
	using Core.Providers;

	[ApiController]
	public class MangaController : ControllerBase
	{
		private readonly ILogger _logger;
		private readonly IMangakakalot _mangakakalot;

		public MangaController(ILogger<MangaController> logger,	IMangakakalot mangakakalot)
		{
			_logger = logger;
			_mangakakalot = mangakakalot;
		}

		[HttpGet, Route("manga/{id}")]
		[ProducesDefaultResponseType(typeof(Manga))]
		public async Task<IActionResult> Get(string id)
		{
			var manga = await _mangakakalot.Manga(id);
			if (manga == null) return NotFound();

			return Ok(manga);
		}

		[HttpGet, Route("manga/{id}/{chapter}")]
		[ProducesDefaultResponseType(typeof(ChapterPages))]
		public async Task<IActionResult> Get(string id, string chapter)
		{
			var chap = await _mangakakalot.ChapterPages(id, chapter);
			if (chap == null) return NotFound();

			return Ok(chap);
		}
	}
}