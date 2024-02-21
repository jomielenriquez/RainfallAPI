using Microsoft.AspNetCore.Mvc;
using RainfallAPI.components.responses;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace RainfallAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RainfallController : ControllerBase
    {
        private readonly ILogger<RainfallController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public RainfallController(ILogger<RainfallController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("id/{stationId}/readings", Name = "get-rainfall")]
        [Produces("application/json")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        [Consumes("application/json")]
        [SwaggerOperation(
            Summary = "Get rainfall readings by station Id",
            Description = "Retrieve the latest readings for the specified stationId",
            OperationId = "get-rainfall"
        )]
        [SwaggerResponse(200, "A list of rainfall readings successfully retrieved", typeof(rainfallReadingResponse))]
        [SwaggerResponse(400, "Invalid request", typeof(ErrorResponse))]
        [SwaggerResponse(404, "No readings found for the specified stationId", typeof(ErrorResponse))]
        [SwaggerResponse(500, "Internal server error", typeof(ErrorResponse))]
        public async Task<IActionResult> GetRainfallReadings(
            [FromRoute][SwaggerParameter("The id of the reading station.")]
            string stationId, 
            [FromQuery][Range(1,100, ErrorMessage = "Count must be between 1 to 100.")] double? count)
        {
            try
            {
                var apiUrl = $"https://environment.data.gov.uk/flood-monitoring/id/stations/{stationId}/readings";

                if (count.HasValue)
                {
                    apiUrl += $"?_sorted&_limit={count.Value}";
                }
                else
                {
                    apiUrl += "?_sorted";
                }

                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    
                    return Ok(content);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound(new ErrorResponse { Message = "No readings found for the specified stationId", Detail = new List<string> { "404" } });
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    return BadRequest(new ErrorResponse { Message = "Invalid request", Detail = new List<string>{"400"} });
                }
                else
                {
                    var errorResponse = new ErrorResponse
                    {
                        Message = "Internal server error",
                        Detail = new List<string> { $"{(int)response.StatusCode}" }
                    };
                    _logger.LogError($"Failed to fetch data. Status code: {response.StatusCode}");
                    return StatusCode((int)response.StatusCode, errorResponse);
                }
            }
            catch (HttpRequestException ex)
            {
                var errorResponse = new ErrorResponse
                {
                    Message = "An error occurred while fetching data",
                    Detail = new List<string> { ex.Message }
                };
                _logger.LogError($"An error occurred while fetching data: {ex.Message}");
                return StatusCode(500, errorResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponse
                {
                    Message = "An unexpected error occurred",
                    Detail = new List<string> { ex.Message }
                };
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return StatusCode(500, errorResponse);
            }
        }
    }
}