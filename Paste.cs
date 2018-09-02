using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ToPasteBin
{
    public class Paste
    {
        [JsonProperty("paste_key")]
        public string Key { get; set; }
        [JsonProperty("paste_date")]
        public string Date { get; set; }
        [JsonProperty("paste_title")]
        public string Title { get; set; }
        [JsonProperty("paste_size")]
        public string Size { get; set; }
        [JsonProperty("paste_expire_date")]
        public string ExpireDate { get; set; }
        [JsonProperty("paste_private")]
        public string Private { get; set; }
        [JsonProperty("paste_format_long")]
        public string FormatLong { get; set; }
        [JsonProperty("paste_format_short")]
        public string FormatShort { get; set; }
        [JsonProperty("paste_url")]
        public string Url { get; set; }
        [JsonProperty("paste_hits")]
        public string Hits { get; set; }
    }
}
