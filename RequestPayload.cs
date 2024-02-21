using System.ComponentModel.DataAnnotations;

namespace RainfallAPI
{
    public class RequestPayload
    {
        public int stationId { get; set; }

        [Range(1, 100, ErrorMessage = "Limit must be between 1 and 100.")]
        public int? limit { get; set; }
    }
}
