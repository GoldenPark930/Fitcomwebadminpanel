namespace LinksMediaCorpEntity
{
    using System.Collections.Generic;
    /// <summary>
    /// Classs for get Trainer details in web api
    /// </summary>
    public class TrainerInfo
    {
        public string Title { get; set; }

        public string TrainerName { get; set; }

        public List<string> TrainerSpecialities { get; set; }

        public string HashTag { get; set; }

        public int IsVerifiedTrainer { get; set; }

        public string TrainerImageURL { get; set; }

        public int TrainerId { get; set; }

        public string UserType { get; set; }

        public string TrainerType { get; set; } 

        public Bio BioData { get; set; }

        public long NotificationID { get; set; }
        
    }
}