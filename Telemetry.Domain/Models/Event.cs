using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telemetry.Domain.Models
{
    public class Event
    {
        public string CustomerId { get; set; } = "";
        public string DeviceId { get; set; } = "";
        public string EventId { get; set; } = "";
        public DateTime RecordedAt { get; set; }
        public string Type { get; set; } = "temperature";
        public double Value { get; set; }
        public string Unit { get; set; } = "C";
    }
}
