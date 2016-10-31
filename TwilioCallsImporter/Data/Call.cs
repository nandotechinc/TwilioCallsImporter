using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwilioCallsImporter.Data
{
    public class Call
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string IpAddress { get; set; }
        public string CallId { get; set; }
        public string SourceNumber { get; set; }
        public string DestinationNumber { get; set; }
        public int Duration { get; set; }
        public float CallRate { get; set; }
        public float CallCost { get; set; }
        public DateTime InsertDate { get; set; }
    }
}
