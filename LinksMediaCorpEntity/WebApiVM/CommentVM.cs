namespace LinksMediaCorpEntity
{
    using System;
    /// <summary>
    /// Class for Resonse of comment
    /// </summary>
    public class CommentVM
    {
        public int CommentId { get; set; }

        public int PostId { get; set; }

        public int CommentBy { get; set; }

        public string ImageUrl { get; set; }

        public string UserName { get; set; }

        public string Message { get; set; }

        public string CommentDate { get; set; }

        public int UserId { get; set; }

        public string UserType { get; set; }

        public DateTime PostedCommentDate { get; set; }
    }
}