using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace RainfallAPI.components
{
    public class rainfallReading
    {
        [JsonProperty("dateMeasured")]
        public string DateMeasured { get; set; }
        [JsonProperty("amountMeasured")]
        public double AmountMeasured { get; set; }
    }
}
