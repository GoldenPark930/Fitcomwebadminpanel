namespace LinksMediaCorpEntity
{
    using System;
    /// <summary>
    /// Classs for Response of Video and Pic
    /// </summary>
    public class VideoAndPicVM
    {
        public int RecordId { get; set; }

        public string MediaUrl { get; set; }

        public string MediaType { get; set; }

        public string ThumbnailUrl { get; set; }

        public DateTime PostedDate { get; set; }

        public string Height { get; set; }

        public string Width { get; set; } 
    }
}