using System;
namespace LinksMediaCorpEntity
{
   /// <summary>
   /// Classs for Team User Response
   /// </summary>
   public class TeamUserVM
    {
        public int ID { get; set; }

        public int CredID { get; set; }

        public int UserId { get; set; }

        public string UserType { get; set; }

        public string FullName { get; set; }

        public string ImageUrl { get; set; }

        public bool IsPersonalTrainer { get; set; }

        public bool IsMTActive { get; set; } 

        public int PersonalTrainerId { get; set; }

        public int CompletedAssignmentCount { get; set; }  
         
        public string EmailId { get; set; }

        public int OffLineChatCount { get; set; }

        public DateTime  LatestTrainerUserNotifyDateTime { get; set; }       
       
    }
}
