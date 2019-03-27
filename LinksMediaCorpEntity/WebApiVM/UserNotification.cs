namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Request for User Notification
    /// </summary>
    public class UserNotification 
    {
        public int UserCredID { get; set; }

        public string DeviceID { get; set; }

        public string DeviceType { get; set; }

        public int TokenId { get; set; } 
    }
}