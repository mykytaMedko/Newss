using Newss.Core.Constants;
using System.Text.Json.Serialization;

namespace Newss.Core.Models
{
    internal class ApiResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("code")]
        public ErrorCodes? Code { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("articles")]
        public List<Article> Articles { get; set; }
        [JsonPropertyName("totalResults")]
        public int TotalResults { get; set; }
    }
}