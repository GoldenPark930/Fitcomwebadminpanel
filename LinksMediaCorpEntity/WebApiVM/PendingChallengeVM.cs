
namespace LinksMediaCorpEntity
{
    using System;
    using System.Collections.Generic;
    /// <summary>
    /// Classs for response of Pending Challenge
    /// </summary>
    public class PendingChallengeVM
    {
        public int ChallengeId { get; set; }

        public int ChallengeToFriendId { get; set; }

        public int ChallengeSubTypeId { get; set; }

        public int SubjectId { get; set; }

        public string ChallengeName { get; set; }

        public string DifficultyLevel { get; set; }

        public string ChallengeType { get; set; }

        public string Equipment { get; set; }

        public string TargetZone { get; set; }

        public List<string> TempTargetZone { get; set; }

        public List<string> TempEquipments { get; set; }

        public int Strenght { get; set; }

        public string ResultUnit { get; set; }

        public string ResultUnitSuffix { get; set; }

        public string ChallengeByUserName { get; set; }

        public string Result { get; set; }

        public string Fraction { get; set; }

        public float TempOrderIntValue { get; set; }

        public string PersonalBestResult { get; set; }

        public string Duration { get; set; }

        public bool IsRecentChallengUserBest { get; set; }

        public List<ChallengeByUser> ChallengeBy { get; set; }

        public Nullable<DateTime> DbPostedDate { get; set; }

        public string Description { get; set; }

        public string ChallengeByUserType { get; set; }

        public int ChallengeByUserId { get; set; }

        public bool IsWellness { get; set; }

        public string ChallengedUserImageUrl { get; set; }

        public string ProgramImageUrl { get; set; }

        public string PersonalMessage { get; set; }

        public string ThumbnailUrl { get; set; }

        public string Height { get; set; }

        public string Width { get; set; }

        public string ThumbNailHeight { get; set; }

        public string ThumbNailWidth { get; set; }

        public bool IsSubscription { get; set; }

        public bool IsActiveProgram { get; set; }
    }
    
}