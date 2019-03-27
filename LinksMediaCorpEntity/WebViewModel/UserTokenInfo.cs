namespace LinksMediaCorpEntity
{
    using System;
    /// <summary>
    /// classs for user TokenInfo in web api
    /// </summary>
    public class UserTokenInfo
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Token { get; set; }

        public bool IsExpired { get; set; }

        public DateTime ExpiredOn { get; set; }
    }
}