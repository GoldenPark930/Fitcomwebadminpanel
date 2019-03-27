namespace LinksMediaCorpEntity
{
    using System.Collections.Generic;
    /// <summary>
    /// Classs for Data trafer of Team Trainer details
    /// </summary>
    public class TeamTrainerVM
    {
        public int TrainerId { get; set; }

        public int CredTrainerId { get; set; }

        public string TrainerName { get; set; }

        public string ImageUrl { get; set; }

        public byte IsVerifiedTrainer { get; set; }

        public List<string> Specilaization { get; set; }

        public int TeamMemberCount { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public JoinStatus Status { get; set; }

        public string HashTag { get; set; }

        public string TeamName { get; set; } 

        public string UserType { get; set; }

        public string TrainerType { get; set; }

        public int TeamId { get; set; }

        public string EmailId { get; set; } 
    }
   
}