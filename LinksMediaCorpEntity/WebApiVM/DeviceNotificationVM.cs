namespace LinksMediaCorpEntity
{
    /// <summary>
    /// class for Resonse of Device Notification
    /// </summary>
    public class DeviceNotificationVM
    {
        public int UserId { get; set; }

        public string UserType { get; set; }

        public string DeviceToken { get; set; }

        public string DeviceType { get; set; }

        public int TotalNotificationCount { get; set; }
    }
}