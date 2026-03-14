using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Application.Interfaces;
using CarpoolingSystem.Infrastructure.Configuration;
using CarpoolingSystem.Infrastructure.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace CarpoolingSystem.Infrastructure.ExternalServices {
    public class ReverseGeoCodingService : IReverseGeocodingService {
        private readonly HttpClient _httpClient;
        private readonly ReverseGeoCodingOptions _options;

        public ReverseGeoCodingService(HttpClient httpClient, IOptions<ReverseGeoCodingOptions> options) {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<LocationDto?> GetLocationAsync(double latitude, double longitude) {
            var url = $"{_options.BaseUrl}?latitude={latitude}&longitude={longitude}&localityLanguage=en&key={_options.ApiKey}";
            var response = await _httpClient.GetFromJsonAsync<ReverseGeoCodingResponseModel>(url);

            return new LocationDto {
                City = response?.City,
                State = response?.State,
            };
        }
    }
}
