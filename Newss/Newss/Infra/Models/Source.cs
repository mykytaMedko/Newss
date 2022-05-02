using System.Text.Json.Serialization;

namespace Newss.Infra.Models
{
    public class Source
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
