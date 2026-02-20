using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telemetry.Domain.Models
{
    public class Insight
    {
        public int WindowHours { get; set; }
        public Event Latest { get; set; } = new Event();
        public Stat Stats { get; set; } = new Stat();

    }
}
 