
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Response of Notification Setting
    /// </summary>
    public class NotificationSettingVM
    {
       private bool notifyTrainerChallege = true;
       private bool notifyFriendChallege = true;
       private bool notifyTrainerJoinTeam = true;  

       public bool TrainerChallegeNotify
       {
           get {  return notifyTrainerChallege; }
           set { notifyTrainerChallege = value; }
       }
       public bool FriendChallegeNotify
       {
           get { return notifyFriendChallege; }
           set { notifyFriendChallege = value; }
       }
       public bool TrainerJoinTeamNotify
       {
           get { return notifyTrainerJoinTeam; }
           set { notifyTrainerJoinTeam = value; }
       } 
        
    }
}