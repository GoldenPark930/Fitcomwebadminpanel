namespace LinksMediaCorpEntity
{
    using System.Web.Mvc;
    /// <summary>
    /// Classs for get Chalenge CategoryDetail on View in Admin
    /// </summary>
    public class ChalengeCategoryDetailView
    {
        public int ChallengeCategoryId { get; set; }

        public string CategoryName { get; set; }

        public string SubCategoryTittle { get; set; }

        public bool TrainerShow { get; set; }

        public bool TrainingZoneShow { get; set; }

        public bool ExerciseTypeShow { get; set; }

        public bool IsSubCategoryExist { get; set; } 

        public SelectList ChallengeSubCategoryList { get; set; }
    }
}