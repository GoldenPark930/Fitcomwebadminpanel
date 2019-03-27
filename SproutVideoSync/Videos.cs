using Newtonsoft.Json;

namespace SproutVideoSync
{
    public class Videos
    {
        [JsonProperty("240p")]
        public string _240p { get; set; }
        [JsonProperty("360p")]
        public string _360p { get; set; }
        [JsonProperty("480p")]
        public string _480p { get; set; }
        [JsonProperty("720")]
        public string _720p { get; set; }
        [JsonProperty("1080p")]
        public string _1080p { get; set; }
        [JsonProperty("_2k")]
        public object _2k { get; set; }
        [JsonProperty("_4k")]
        public object _4k { get; set; }
        [JsonProperty("_8k")]
        public object _8k { get; set; }
        public object source { get; set; }
    }
}
