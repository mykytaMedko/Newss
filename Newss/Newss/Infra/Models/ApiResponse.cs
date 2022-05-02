using Newss.Infra.Constants;
using System.Text.Json.Serialization;

namespace Newss.Infra.Models
{
    public class ApiResponse
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