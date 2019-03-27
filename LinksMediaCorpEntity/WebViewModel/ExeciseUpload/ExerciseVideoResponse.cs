using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LinksMediaCorpEntity
{
    public class RootObject
    {
        public int total { get; set; }
        public List<ExerciseVideoResponse> videos { get; set; }
        public string next_page { get; set; }
    }
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

        //public object __invalid_name__240p { get; set; }
        //public object __invalid_name__360p { get; set; }
        //public object __invalid_name__480p { get; set; }
        //public object __invalid_name__720p { get; set; }
        //public object __invalid_name__1080p { get; set; }
        //public object __invalid_name__2k { get; set; }
        //public object __invalid_name__4k { get; set; }
        //public object __invalid_name__8k { get; set; }
        //public object source { get; set; }
    }

    public class Assets
    {
        public Videos videos { get; set; }
        public List<string> thumbnails { get; set; }
        public List<string> poster_frames { get; set; }
    }
    public class ExerciseVideoResponse
    {
        public string id { get; set; }
        public object width { get; set; }
        public object height { get; set; }
        public string embed_code { get; set; }
        public int source_video_file_size { get; set; }
        public int sd_video_file_size { get; set; }
        public int hd_video_file_size { get; set; }
        public string security_token { get; set; }
        public string title { get; set; }
        public object description { get; set; }
        public object duration { get; set; }
        public int privacy { get; set; }
        public object password { get; set; }
        public string state { get; set; }
        public List<object> tags { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public int plays { get; set; }
        public double progress { get; set; }
        public bool requires_signed_embeds { get; set; }
        public int selected_poster_frame_number { get; set; }
        public object embedded_url { get; set; }
        public Assets assets { get; set; }
        public object download_sd { get; set; }
        public object download_hd { get; set; }
        public object download_source { get; set; }
        public object allowed_domains { get; set; }
        public object allowed_ips { get; set; }
        public object player_social_sharing { get; set; }
        public object player_embed_sharing { get; set; }
        public bool require_email { get; set; }
        public bool hide_on_site { get; set; }
        public string checksum { get; set; }
    }
}
