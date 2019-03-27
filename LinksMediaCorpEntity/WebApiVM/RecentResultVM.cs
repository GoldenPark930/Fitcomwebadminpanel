namespace LinksMediaCorpEntity
{
    using System;
    using System.Collections.Generic;
    /// <summary>
    /// Class for Response of Recent Result
    /// </summary>
    public class RecentResultVM
    {
        public int ResultId { get; set; }

        public int userID { get; set; }

        public int UserCredID { get; set; }   

        public int ChallengeId { get; set; }

        public string ChallengeName { get; set; }

        public string DifficultyLevel { get; set; }

        public string Equipments { get; set; } 

        public string TargetZone { get; set; }

        public List<string> TempTargetZone { get; set; }

        public List<string> TempEquipments { get; set; } 

        public int Strength { get; set; } 

        public string Result { get; set; }

        public string Fraction { get; set; }

        public int ChallengeSubTypeid { get; set; }

        public string ResultUnit { get; set; }

        public float TempOrderIntValue { get; set; }

        public string PersonalBestResult { get; set; } 

        public bool IsRecentChallengUserBest {get;set;}

        public string UserImageUrl { get; set; }

        public string UserName { get; set; }

        public string UserType { get; set; }

        public DateTime DbPostedDate { get; set; }

        public int BoomsCount { get; set; }

        public int CommentsCount { get; set; }

        public bool IsLoginUserBoom { get; set; }

        public bool IsLoginUserComment { get; set; }

        public string ResultUnitSuffix { get; set; }

        public string ChallengeType { get; set; }

        public string PostType { get; set; }

        public int PostId { get; set; }

        public string PostedDate { get; set; }   
                  
        public string Message { get; set; }   
          
        public int PostedBy { get; set; }

        public List<VideoInfo> VideoList { get; set; }

        public List<PicsInfo> PicList { get; set; }    
           
        public int TargetID { get; set; }

        public string TargetType { get; set; }

        public bool IsJoinedTeam { get; set; }

        public bool isWellness { get; set; }

        public string Duration { get; set; }

        public ExeciseVideoDetail ExeciseVideoDetails { get; set; } 

        public string ThumbnailUrl { get; set; }

        public string Description { get; set; }

        public string ThumbNailHeight { get; set; }

        public string ThumbNailWidth { get; set; }

        public string ExeciseVideoLink { get; set; }

        public bool IsSubscription { get; set; }      
    }   
}