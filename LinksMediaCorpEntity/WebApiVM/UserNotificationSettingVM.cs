namespace LinksMediaCorpEntity
{
    /// <summary>
    /// User Notification View Model
    /// </summary>
    public class UserNotificationSettingVM
    {
        public string DeviceID { get; set; }

        public NotificationType NotificationType { get; set; }

        public bool IsNotify { get; set; }

        public DeviceType DeviceType { get; set; }
    }
}
  