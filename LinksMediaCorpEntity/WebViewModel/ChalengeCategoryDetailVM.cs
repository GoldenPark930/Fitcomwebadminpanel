namespace LinksMediaCorpEntity
{
    using System.Collections.Generic;
    /// <summary>
    /// Class for  get Chalenge Category Detail on Admin
    /// </summary>
    public class ChalengeCategoryDetailVM
    {       
        public int ChallengeCategoryId { get; set; }

        public string CategoryName { get; set; }

        public string SubCategoryTittle { get; set; }

        public bool TrainerShow { get; set; }

        public bool TrainingZoneShow { get; set; }

        public bool ExerciseTypeShow { get; set; }

        public bool IsSubCategoryExist { get; set; }

        public List<ChalengeSubCategoryVM> ChallengeSubCategoryList { get; set; }       
    }
}