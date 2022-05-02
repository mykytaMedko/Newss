using System.Globalization;
using System.Text.Json.Serialization;

namespace NewssCore.Models
{
    public class Article
    {
        [JsonPropertyName("source")]
        public Source Source { get; set; }
        [JsonPropertyName("author")]
        public string Author { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("urlToImage")]
        public string UrlToImage { get; set; }
        [JsonPropertyName("publishedAt")]
        public DateTime? PublishedAt { get; set; }
        [JsonPropertyName("content")]
        public string Content { get; set; }
        public string AuthorAndTime { get; set; }

        public void SetAuthorAndTime()
        {
            var author = Author;
            var time = GetTime(PublishedAt);
            if (author != null && time != null)
            {
                AuthorAndTime = $"{author} : {time}";
            }
            else
            {
                if (author != null)
                {
                    AuthorAndTime = author;
                }
                if (time != null)
                {
                    AuthorAndTime = time;
                }
            }
        }

        private string GetTime(DateTime? dateTime)
        {
            if (dateTime == null)
                return null;
            var diff = DateTime.Now - dateTime.Value;
            if (diff.Days == 0)
            {
                return $"{diff.Hours} годин тому";
            }
            if (diff.Days <= 30)
            {
                return $"{diff.Days} днів тому";
            }
            if (diff.Days >= 30)
            {
                return $"{diff.Days / 30} місяців тому";
            }
            else
            {
                return dateTime.Value.ToString("HH:mm dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk"));
            }
        }
    }
}
