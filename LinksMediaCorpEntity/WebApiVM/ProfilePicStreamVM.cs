namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Response of Profile PicStream
    /// </summary>
    public class ProfilePicStreamVM
    {
        public int UserId { get; set; }

        public string UserType { get; set; }

        public string PicBase64String { get; set; }

        public string PicName { get; set; }

        public string PicExtension { get; set; }
    }
}