namespace LinksMediaCorpEntity
{
    using System;
    /// <summary>
    /// Classs for View Activities in admin
    /// </summary>
    public class ViewActivitiesVM
    {
        public int ActivityId { get; set; }       

        public string NameOfActivity { get; set; }

        public string TrainerName { get; set; }     

        public DateTime DateofEvent { get; set; }

        public string Location { get; set; }

        public string PromotionText { get; set; } 
    }
}