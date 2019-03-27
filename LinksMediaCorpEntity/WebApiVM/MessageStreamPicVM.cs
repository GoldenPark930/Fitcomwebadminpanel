namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Class for Response of MessageStream Pic
    /// </summary>
    public class MessageStreamPicVM
    {
        public int MessageStreamId { get; set; }

        public string PicBase64String { get; set; }

        public string PicName { get; set; }

        public string PicExtension { get; set; }

        public string ImageMode { get; set; }

        public string Height { get; set; }

        public string Width { get; set; }  
    }
}