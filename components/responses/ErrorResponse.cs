using Newtonsoft.Json;

namespace RainfallAPI.components.responses
{
    public class ErrorResponse
    {
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("detail")]
        public List<string> Detail { get; set; }
    }
}
