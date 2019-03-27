namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Class for Resonse of MessageStream Pic
    /// </summary>
    public class MessageStreamVideoVM
    {
        public int MessageStreamId { get; set; }     
         
        public byte[] VideoByteArray { get;   set; }

        public string VideoName { get; set; }

        public string VideoExtension { get; set; }

        public bool IsChunkedData { get; set; }

        public bool IsLastChunk { get; set; }

        public string ThumbNail { get; set; }     
    }      
}