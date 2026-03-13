using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Infrastructure.Configuration {
    public class ReverseGeoCodingOptions {
        public string ApiKey { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = string.Empty;
    }
}