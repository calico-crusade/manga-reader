using CardboardBox.Http;
using HtmlAgilityPack;

namespace MangaReader.Core.Providers
{
	public interface IMangakakalot : IMangaSource { }

	public class Mangakakalot : IMangakakalot
	{
		private readonly Dictionary<string, CacheItem<Manga?>> _mangaCache = new();
		private readonly Dictionary<string, CacheItem<ChapterPages?>> _chapterCache = new();

		public string HomeUrl => "https://ww1.mangakakalot.tv/";

		public string ChapterBaseUri => $"{HomeUrl}chapter/";

		public string MangaBaseUri => $"{HomeUrl}manga/";

		public string Provider => "Mangakakalot.tv";

		private readonly IApiService _api;

		public Mangakakalot(IApiService api)
		{
			_api = api;
		}

		public async Task<HtmlDocument?> GetHtml(string url)
		{
			var req = await _api.Create(url).Result();
			if (req == null) return null;

			req.EnsureSuccessStatusCode();
			var html = await req.Content.ReadAsStringAsync();

			var doc = new HtmlDocument();
			doc.LoadHtml(html);

			return doc;
		}

		public Task<ChapterPages?> ChapterPages(string mangaId, string chapterId)
		{
			var id = $"{mangaId}/{chapterId}";
			if (!_chapterCache.ContainsKey(id))
				_chapterCache.Add(id, new CacheItem<ChapterPages?>(() => RawChapterPages(mangaId, chapterId)));

			return _chapterCache[id].Get();
		}

		public async Task<ChapterPages?> RawChapterPages(string mangaId, string chapterId)
		{
			var url = $"{ChapterBaseUri}{mangaId}/{chapterId}";
			var doc = await GetHtml(url);
			if (doc == null) return null;

			var chapter = new ChapterPages
			{
				Id = chapterId,
				Url = url,
				Number = double.TryParse(url.Split('-').Last(), out var n) ? n : 0
			};

			chapter.Title = doc
				.DocumentNode
				.SelectSingleNode("//div[@class=\"rdfa-breadcrumb\"]/div/p")
				.ChildNodes
				.Where(t => t.Name == "span")
				.Last()
				.SelectSingleNode("./a/span")
				.InnerText.Trim();

			chapter.Pages = doc
				.DocumentNode
				.SelectNodes("//div[@class=\"vung-doc\"]/img[@class=\"img-loading\"]")
				.Select(t => t.GetAttributeValue("data-src", ""))
				.ToArray();

			return chapter;
		}

		public Task<Manga?> Manga(string id)
		{
			if (!_mangaCache.ContainsKey(id))
				_mangaCache.Add(id, new CacheItem<Manga?>(() => RawManga(id)));

			return _mangaCache[id].Get();
		}

		public async Task<Manga?> RawManga(string id)
		{
			var url = $"{MangaBaseUri}{id}";
			var doc = await GetHtml(url);
			if (doc == null) return null;

			var manga = new Manga
			{
				Title = doc.DocumentNode.SelectSingleNode("//ul[@class=\"manga-info-text\"]/li/h1").InnerText,
				Id = id,
				Provider = Provider,
				HomePage = url,
				Cover = HomeUrl + doc.DocumentNode.SelectSingleNode("//div[@class=\"manga-info-pic\"]/img").GetAttributeValue("src", "").TrimStart('/')
			};

			var textEntries = doc.DocumentNode.SelectNodes("//ul[@class=\"manga-info-text\"]/li");

			foreach(var li in textEntries)
			{
				if (!li.InnerText.StartsWith("Genres")) continue;

				var atags = li.ChildNodes.Where(t => t.Name == "a").Select(t => t.InnerText).ToArray();
				manga.Tags = atags;
				break;
			}

			var chapterEntries = doc.DocumentNode.SelectNodes("//div[@class=\"chapter-list\"]/div[@class=\"row\"]");

			foreach(var chapter in chapterEntries)
			{
				var a = chapter.SelectSingleNode("./span/a");
				var href = HomeUrl + a.GetAttributeValue("href", "").TrimStart('/');
				var num = double.TryParse(href.Split('-').Last(), out var n) ? n : 0;
				var c = new Chapter
				{
					Title = a.InnerText.Trim(),
					Url = href,
					Number = num,
					Id = href.Split('/').Last()
				};

				manga.Chapters.Add(c);
			}

			return manga;
		}
	}
}
