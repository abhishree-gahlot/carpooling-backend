using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Domain.Entities {
    public class Location {
        public Guid ID { get; set;  }

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}
