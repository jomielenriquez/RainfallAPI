using Newtonsoft.Json;

namespace RainfallAPI.components.responses
{
    public class rainfallReadingResponse
    {
        [JsonProperty("readings")]
        public List<rainfallReading> Readings { get; set; }
    }
}
