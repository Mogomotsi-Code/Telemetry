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
        public List<Event> Events { get; set; } = new List<Event>();
        public Stat Stast { get; set; } = new Stat();

    }
}
