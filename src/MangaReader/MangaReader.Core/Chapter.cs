using System.Text.Json.Serialization;

namespace MangaReader.Core
{
	public class Chapter
	{
		[JsonPropertyName("title")]
		public string Title { get; set; } = string.Empty;

		[JsonPropertyName("url")]
		public string Url { get; set; } = string.Empty;

		[JsonPropertyName("id")]
		public string Id { get; set; } = string.Empty;

		[JsonPropertyName("number")]
		public double Number { get; set; }
	}

	public class ChapterPages : Chapter
	{
		[JsonPropertyName("pages")]
		public string[] Pages { get; set; } = Array.Empty<string>();
	}
}
