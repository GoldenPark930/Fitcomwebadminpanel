using System.Collections.Generic;

namespace SproutVideoSync
{
    public class Video
    {
        public string id { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string embed_code { get; set; }
        public int source_video_file_size { get; set; }
        public int sd_video_file_size { get; set; }
        public int hd_video_file_size { get; set; }
        public string security_token { get; set; }
        public string title { get; set; }
        public object description { get; set; }
        public double duration { get; set; }
        public int privacy { get; set; }
        public object password { get; set; }
        public string state { get; set; }
        public List<object> tags { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public int plays { get; set; }
        public int progress { get; set; }
        public bool requires_signed_embeds { get; set; }
        public int selected_poster_frame_number { get; set; }
        public object embedded_url { get; set; }
        public Assets assets { get; set; }
        public object download_sd { get; set; }
        public object download_hd { get; set; }
        public object download_source { get; set; }
        public object allowed_domains { get; set; }
        public object allowed_ips { get; set; }
        public bool? player_social_sharing { get; set; }
        public bool? player_embed_sharing { get; set; }
        public bool require_email { get; set; }
        public bool hide_on_site { get; set; }
    }
}
