using System.Collections.Generic;
namespace SproutVideoSync
{
    class SproutResponse
    {
        public int total { get; set; }
        public List<Video> videos { get; set; }
        public string next_page { get; set; }
    }
}
