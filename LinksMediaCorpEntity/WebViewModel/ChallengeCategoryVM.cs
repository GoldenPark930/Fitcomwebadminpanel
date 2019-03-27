namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for get response of Challenge Category
    /// </summary>
    public class ChallengeCategoryVM
    {
        public int ChallengeCategoryId { get; set; }

        public string CategoryName { get; set; }

        public string SubCategoryTittle { get; set; }

        public bool TrainerShow { get; set; }

        public bool TrainingZoneShow { get; set; }

        public bool ExerciseTypeShow { get; set; }

        public bool IsActive { get; set; }
    }
}