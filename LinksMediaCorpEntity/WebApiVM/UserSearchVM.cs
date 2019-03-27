namespace LinksMediaCorpEntity
{
    using System.Collections.Generic;
    /// <summary>
    /// Classs for Resonse for User Search
    /// </summary>
    public class UserSearchVM
    {           
        public int ID { get; set; }     
          
        public string SelectSearchType { get; set; }

        public string FirstName { get; set; } 

        public string FullName { get; set; }

        public string ChallengeName { get; set; } 

        public string ImageUrl { get; set; }  

        public string City { get; set; }

        public string State { get; set; }

        public int TeamMemberCount { get; set; }

        public byte IsVerifiedTrainer { get; set; }

        public List<string> Specilaization { get; set; }     
           
        public string DifficultyLevel { get; set; }

        public string ChallengeType { get; set; }

        public string Equipment { get; set; }

        public List<string> TargetZone { get; set; }

        public int Strenght { get; set; }

        public string ResultUnit { get; set; }

        public string TrainerType { get; set; }

        public int ChallengeSubTypeId { get; set; }

        public bool IsWellness { get; set; }

        public int TrainerId { get; set; } 

        public bool IsPremium { get; set; }

    }
}