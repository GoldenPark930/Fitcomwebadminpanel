using System;
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for get Featured Activity Queue in Admin
    /// </summary>
    public class FeaturedActivityQueue
    {
        public int QueueId { get; set; }

        public int ActivityId { get; set; }

        public string NameOfActivity { get; set; }

        public string TrainerName { get; set; }

        public DateTime DateofEvent { get; set; }

        public string Location { get; set; }

        public string PromotionText { get; set; }
    }
}
