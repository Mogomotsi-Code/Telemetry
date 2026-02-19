using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telemetry.Domain.Models
{
    public class Device
    {
        public string CustomerId { get; set; } = "";
        public string DeviceId { get; set; } = "";
        public string Label { get; set; } = "";
        public string Location { get; set; } = "";
    }
}
