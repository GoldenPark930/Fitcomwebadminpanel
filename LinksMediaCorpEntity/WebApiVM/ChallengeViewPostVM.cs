namespace LinksMediaCorpEntity
{
    using System;
    using System.Collections.Generic;
    /// <summary>
    /// class for Response of Challenge Post
    /// </summary>
    public class ChallengeViewPostVM
    {
        public int PostId { get; set; }

        public string PostedByImageUrl { get; set; }

        public string UserName { get; set; }

        public string PostedDate { get; set; }

        public DateTime DbPostedDate { get; set; }

        public string Message { get; set; }

        public List<VideoInfo> VideoList { get; set; }

        public List<PicsInfo> PicList { get; set; }

        public int BoomsCount { get; set; }

        public bool IsLoginUserBoom { get; set; }

        public bool IsLoginUserComment { get; set; }

        public int CommentsCount { get; set; }

        public int PostedBy { get; set; }

        public int UserId { get; set; }

        public string UserType { get; set; }
    }
}