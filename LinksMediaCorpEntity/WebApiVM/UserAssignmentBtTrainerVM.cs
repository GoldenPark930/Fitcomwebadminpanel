using System;
using System.Collections.Generic;
namespace LinksMediaCorpEntity
{
   /// <summary>
    /// Class for Response of User Assignment By Trainer
   /// </summary>
   public class UserAssignmentByTrainerVM
    {
       public int UserAssignmentUpdateId { get; set; } 

        public int ChallengeId { get; set; }

        public int ChallengeToFriendId { get; set; }

        public int ChallengeSubTypeId { get; set; }

        public int SubjectId { get; set; }

        public int TargetId { get; set; }

        public string ChallengeName { get; set; } 

        public string ChallengeByUserName { get; set; }

        public string ChallengeType { get; set; }

        public string Equipment { get; set; }

        public string TargetZone { get; set; }

        public List<string> TempTargetZone { get; set; }

        public List<string> TempEquipments { get; set; } 

        public List<ChallengeByUser> ChallengeBy { get;  set; }

        public Nullable<DateTime> DbPostedDate { get; set; }

        public Nullable<DateTime> DbCompletedDate { get; set; }

        public string Description { get; set; }   

        public int ChallengeByUserId { get; set; }

        public bool IsWellness { get; set; }

        public bool IsComplete { get; set; } 

        public string ExeciseThumbnailUrl { get; set; } 

        public string ProgramImageUrl { get; set; }

        public string ChallengedUserImageUrl { get; set; }    
           
        public AssignmentStatus Status { get; set; }

        public string ChallengeByUserType { get; set; }

        public string DifficultyLevel { get; set; }

        public string ChallengeDuration { get; set; }

        public string PersonalMessage { get; set; }

        public string ExeciseVideoLink { get; set; } 

        public string Height { get; set; } 

        public string Width { get; set; }  

        public string ThumbNailHeight { get; set; } 

        public string ThumbNailWidth { get; set; }

        public ExeciseVideoDetail ExeciseVideoDetails { get; set; }

        public bool IsActiveProgram { get; set; }
    }    
}
