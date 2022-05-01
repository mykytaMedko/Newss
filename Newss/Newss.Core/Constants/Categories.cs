using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Newss.Core.Constants
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Categories
    {
        Business,
        Entertainment,
        Health,
        Science,
        Sports,
        Technology
    }
}
