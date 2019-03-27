namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Response of Team detail
    /// </summary>
    public class TeamsVM
    {
        public int TeamId { get; set; }

        public int UniqueTeamId { get; set; } 

        public int CredTeamId { get; set; }

        public string TeamName { get; set; } 

        public string ImagePicUrl { get; set; }

        public string ImagePremiumUrl { get; set; }  

        public int TeamMemberCount { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public JoinStatus Status { get; set; }

        public string HashTag { get; set; }

        public int UserId { get; set; } 

        public string UserType { get; set; }

        public string Height { get; set; }

        public string Width { get; set; }
       
       
    }
    
}