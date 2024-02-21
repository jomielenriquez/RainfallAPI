using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace RainfallAPI.components
{
    public class rainfallReading
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public string DateTimeString { get; set; }

        [JsonProperty("dateTime")]
        public string DateMeasured
        {
            get
            {
                return DateTimeString;
            }
            set
            {
                if (DateTime.TryParseExact(value, "yyyy-MM-ddTHH:mm:ss", null, System.Globalization.DateTimeStyles.None, out DateTime result))
                {
                    DateTimeString = result.ToString();
                }
                
                DateTimeString = value;
            }
        }
        [JsonProperty("value")]
        public double AmountMeasured { get; set; }
    }
}
