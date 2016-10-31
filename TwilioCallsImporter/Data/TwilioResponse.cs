using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwilioCallsImporter.Data
{
    public class Result
    {
        public string first_page_uri { get; set; }
        public int end { get; set; }
        public Call[] calls { get; set; }
        public object previous_page_uri { get; set; }
        public string uri { get; set; }
        public int page_size { get; set; }
        public int start { get; set; }
        public string next_page_uri { get; set; }
        public int page { get; set; }
        public class Call
        {
            public string sid { get; set; }
            public string date_created { get; set; }
            public string date_updated { get; set; }
            public object parent_call_sid { get; set; }
            public string account_sid { get; set; }
            public string to { get; set; }
            public string to_formatted { get; set; }
            public string from { get; set; }
            public string from_formatted { get; set; }
            public object phone_number_sid { get; set; }
            public string status { get; set; }
            public string start_time { get; set; }
            public string end_time { get; set; }
            public string duration { get; set; }
            public string price { get; set; }
            public string price_unit { get; set; }
            public string direction { get; set; }
            public object answered_by { get; set; }
            public object annotation { get; set; }
            public string api_version { get; set; }
            public object forwarded_from { get; set; }
            public object group_sid { get; set; }
            public string caller_name { get; set; }
            public string uri { get; set; }
            public Subresource_Uris subresource_uris { get; set; }
        }

        public class Subresource_Uris
        {
            public string notifications { get; set; }
            public string recordings { get; set; }
        }

    }
}
