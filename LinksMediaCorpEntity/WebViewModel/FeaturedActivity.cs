namespace LinksMediaCorpEntity
{
    using System;
    /// <summary>
    /// Classs for get Featured Activity in admin
    /// </summary>
    public class FeaturedActivity
    {
        public int IsVerifiedTrainer { get; set; }

        public string TrainerImageURL { get; set; }

        public int TrainerId { get; set; }

        public string UserType { get; set; }

        public string Location { get; set; }

        public int FeaturedActivityId { get; set; }

        public string NameOfActivity { get; set; }

        public string Featuring { get; set; }

        public DateTime DbDateOfEvent { get; set; }

        public string DateOfEvent { get; set; }

        public int StrengthCount { get; set; }

        public string PromotionCode { get; set; }

        public string LearnMore { get; set; }

        public string TrainerType { get; set; }
        
    }
}