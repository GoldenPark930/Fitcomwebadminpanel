using System.Collections.Generic;

namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Class for Response for User follwings and Follower detail
    /// </summary>
    public class FollowerFollwingUserVM
    {
        public int ID { get; set; }

        public int CredID { get; set; }

        public string UserType { get; set; } 

        public string FullName { get; set; }

        public string ImageUrl { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public int TeamMemberCount { get; set; }

        public int TrainerTeamCount { get; set; } 

        public byte IsVerifiedTrainer { get; set; }

        public List<string> Specilaization { get; set; } 
                    
        public JoinStatus Status { get; set; }

        public string HashTag { get; set; }

        public string TrainerType { get; set; }

        public string TeamName { get; set; }     
    }
}