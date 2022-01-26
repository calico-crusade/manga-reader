namespace MangaReader.Core
{
	public interface IMangaSource
	{
		public string HomeUrl { get; }
		public string Provider { get; }

		public Task<Manga?> Manga(string id);

		public Task<ChapterPages?> ChapterPages(string mangaId, string chapterId);
	}
}
