namespace LinksMediaCorpEntity
{
    using System.Collections.Generic;
    /// <summary>
    /// Classs for Resonse of Bio
    /// </summary>
    public class Bio
    {
        public string Gender { get; set; }

        public string Age { get; set; }

        public string Height { get; set; }

        public string Weight { get; set; }

        public string AboutME { get; set; }

        public int IsVerifiedTrainer { get; set; }

        public string TrainerImageURL { get; set; }

        public int TrainerId { get; set; }

        public List<string> SpecializationList { get; set; }

        public string TrainerType { get; set; }        
    }
}