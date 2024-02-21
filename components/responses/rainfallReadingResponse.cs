using Newtonsoft.Json;

namespace RainfallAPI.components.responses
{
    public class rainfallReadingResponse
    {
        [JsonProperty("items")]
        public List<rainfallReading> Readings { get; set; }
    }
}
