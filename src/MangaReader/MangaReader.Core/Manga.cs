using System.Text.Json.Serialization;

namespace MangaReader.Core
{
	public class Manga
	{
		[JsonPropertyName("title")]
		public string Title { get; set; } = string.Empty;

		[JsonPropertyName("id")]
		public string Id { get; set; } = string.Empty;

		[JsonPropertyName("provider")]
		public string Provider { get; set; } = string.Empty;

		[JsonPropertyName("homePage")]
		public string HomePage { get; set; } = string.Empty;

		[JsonPropertyName("cover")]
		public string Cover { get; set; } = string.Empty;

		[JsonPropertyName("tags")]
		public string[] Tags { get; set; } = Array.Empty<string>();

		[JsonPropertyName("chapters")]
		public List<Chapter> Chapters { get; set; } = new();
	}
}