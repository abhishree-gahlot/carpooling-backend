using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CarpoolingSystem.Infrastructure.Models {
    public class ReverseGeoCodingResponseModel {
        [JsonPropertyName("city")]
        public string? City { get; set; }

        [JsonPropertyName("principalSubdivision")]
        public string? State { get; set; }
    }
}
