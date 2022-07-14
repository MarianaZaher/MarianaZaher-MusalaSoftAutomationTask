using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusalaSoftAutomationTask.Tests.Pages
{
    class PositionDetails
    {

        [JsonProperty("Position")]
        public string Position { get; set; }

        [JsonProperty("More_info")]
        public string More_info { get; set; }

        [JsonProperty("city")]
        public string city { get; set; }
    }
}
