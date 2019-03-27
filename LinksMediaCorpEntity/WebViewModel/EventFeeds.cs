namespace LinksMediaCorpEntity
{
    using System;
    using System.Collections.Generic;
    /// <summary>
    /// Classs for Add EventFeeds in admin
    /// </summary>
    public class EventFeeds
    {        
        public string UserId { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public string ImageUrl { get; set; }

        public string Message { get; set; }

        public List<string> CommentMessage { get; set; }

        public List<string> BoomMessage { get; set; }
    }
}